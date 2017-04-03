using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IRSI.Services.Models.Data.Common;

namespace IRSI.Services.Api.Data
{
    public class CommonContext : DbContext
    {
        public CommonContext(DbContextOptions<CommonContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>();
            modelBuilder.Entity<Region>();
            modelBuilder.Entity<Concept>();

            modelBuilder.Entity<DayPart>();
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Concept> Concepts { get; set; }

        public virtual DbSet<DayPart> DayParts { get; set; }
    }
}