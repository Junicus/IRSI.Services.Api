using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using IRSI.Services.Api.Configuration.SOSServiceBusService;
using IRSI.Services.Api.Data;
using IRSI.Services.Api.Configuration;
using IRSI.Services.Api.Services;
using IRSI.Services.Api.Security.Handlers;

namespace IRSI.Services.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddLogging();

            services.Configure<SOSServiceBusServiceOptions>(x =>
            {
                x.StorageConnectionString = Configuration["Azure:Storage:StorageConnectionString"];
                x.DashboardConnectionString = Configuration["Azure:Storage:DashboardConnectionString"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanCallSOS", pa =>
                {
                    pa.RequireClaim("sosApiEvent", "true");
                });

                options.AddPolicy("CanCallTeam", pa =>
                {
                    pa.RequireClaim("teamApiEvent", "true");
                });
            });

            services.AddDbContext<CommonContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CommonConnection"), b => b.MigrationsAssembly("IRSI.Services.Api"));
            });

            services.AddDbContext<SOSContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SOSConnection"), b => b.MigrationsAssembly("IRSI.Services.Api"));
            });

            services.AddDbContext<AVTContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AVTConnection"), b => b.MigrationsAssembly("IRSI.Services.Api"));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<TeamSalesDbContextFactoryOptions>(x =>
            {
                x.ConnectionString["cgb"] = Configuration.GetConnectionString("TeamConnectionCGB");
                x.ConnectionString["rmg"] = Configuration.GetConnectionString("TeamConnectionRMG");
                x.ConnectionString["otb"] = Configuration.GetConnectionString("TeamConnectionOTB");
                x.ConnectionString["pfc"] = Configuration.GetConnectionString("TeamConnectionPFC");
            });
            services.AddSingleton<IDbContextFactory<TeamSalesDbContext>, TeamSalesDbContextFactory>();

            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            services.AddSingleton<IAuthorizationHandler, StoreAccessHandler>();

            services.AddMvc();

            services.AddTransient<ISOSServiceBusService, SOSServiceBusService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var jwtBearerOptions = new JwtBearerOptions
            {
                Authority = Configuration["IdentityServer:Authority"],
                Audience = Configuration["IdentityServer:Audience"],
                AutomaticAuthenticate = true
            };

            if (env.IsDevelopment())
            {
                jwtBearerOptions.RequireHttpsMetadata = false;
            }
            else
            {
                jwtBearerOptions.RequireHttpsMetadata = true;
            }

            app.UseJwtBearerAuthentication(jwtBearerOptions);

            app.UseMvc();
        }
    }
}
