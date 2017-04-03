using IRSI.Services.Api.Models.Data.AVT;
using IRSI.Services.Models.Data.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Data
{
    public class AVTContext : DbContext
    {
        public AVTContext(DbContextOptions<AVTContext> options) : base(options)
        {
        }

        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<AVTItem> AVTItems { get; set; }
    }
}
