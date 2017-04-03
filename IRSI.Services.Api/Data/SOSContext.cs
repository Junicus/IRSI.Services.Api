using IRSI.Services.Api.Models.Data.SOS;
using IRSI.Services.Models.Data.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Data
{
    public class SOSContext : DbContext
    {
        public SOSContext(DbContextOptions<SOSContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SOSSummaryItems>()
                .HasKey(q => new { q.SOSItemId, q.SOSSummaryId });

            modelBuilder.Entity<SOSSummaryItems>()
                .HasOne(t => t.Summary).WithMany(t => t.SOSSummaryItems)
                .HasForeignKey(t => t.SOSSummaryId);

            modelBuilder.Entity<SOSSummaryItems>()
                .HasOne(t => t.Item).WithMany(t => t.SOSSummaryItems)
                .HasForeignKey(t => t.SOSItemId);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Concept> Concepts { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Store> Stores { get; set; }

        public virtual DbSet<SOSItem> SOSItems { get; set; }
        public virtual DbSet<SOSAverageTT> SOSAverateTTs { get; set; }
        public virtual DbSet<SOSSummary> SOSSummaries { get; set; }

        public virtual DbSet<SOSSummaryItems> SOSSummaryItems { get; set; }
    }
}
