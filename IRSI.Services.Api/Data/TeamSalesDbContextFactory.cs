using IRSI.Services.Api.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Data
{
    public class TeamSalesDbContextFactory : IDbContextFactory<TeamSalesDbContext>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<TeamSalesDbContextFactoryOptions> _options;

        public TeamSalesDbContextFactory(IOptions<TeamSalesDbContextFactoryOptions> options, IHttpContextAccessor httpContextAccessor)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
        }

        public TeamSalesDbContext Create(DbContextFactoryOptions options)
        {
            var concept = (string)_httpContextAccessor.HttpContext.GetRouteValue("concept");
            switch (concept)
            {
                case "cgb":
                    {
                        var optionsBuilder = new DbContextOptionsBuilder<TeamSalesDbContext>();
                        optionsBuilder.UseSqlServer(_options.Value.ConnectionString["cgb"]);
                        return new TeamSalesDbContext(optionsBuilder.Options);
                    }
                case "rmg":
                    {
                        var optionsBuilder = new DbContextOptionsBuilder<TeamSalesDbContext>();
                        optionsBuilder.UseSqlServer(_options.Value.ConnectionString["rmg"]);
                        return new TeamSalesDbContext(optionsBuilder.Options);
                    }
                case "otb":
                    {
                        var optionsBuilder = new DbContextOptionsBuilder<TeamSalesDbContext>();
                        optionsBuilder.UseSqlServer(_options.Value.ConnectionString["otb"]);
                        return new TeamSalesDbContext(optionsBuilder.Options);
                    }
                case "pfc":
                    {
                        var optionsBuilder = new DbContextOptionsBuilder<TeamSalesDbContext>();
                        optionsBuilder.UseSqlServer(_options.Value.ConnectionString["pfc"]);
                        return new TeamSalesDbContext(optionsBuilder.Options);
                    }
                default:
                    return null;
            }
        }
    }
}
