using IRSI.Services.Api.Models.Data.TeamSales.Aloha;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Data
{
    public class TeamSalesDbContext : DbContext
    {
        public TeamSalesDbContext(DbContextOptions<TeamSalesDbContext> options)
            : base(options)
        {
        }

        public DbSet<ShiftHistory> ShiftHistory { get; set; }
        public DbSet<GrindItemHistory> GrindItemHistory { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<EmployeeByStore> EmployeeByStore { get; set; }
        public DbSet<DateDimension> DateDimensions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeByStore>()
                .HasKey(c => new { c.EmployeeNumber, c.StoreId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
