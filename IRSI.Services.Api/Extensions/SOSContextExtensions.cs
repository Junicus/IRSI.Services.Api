using IRSI.Services.Api.Data;
using IRSI.Services.Api.Models.Data.SOS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Extensions
{
    public static class SOSContextExtensions
    {
        public static decimal GetAverageTT(this SOSContext context, Guid store, DateTime startDate, DateTime endDate)
        {
            var records = from sos in context.SOSItems
                          where sos.StoreId == store && sos.DateOfBusiness >= startDate && sos.DateOfBusiness <= endDate && sos.ItemId == 0
                          select sos;

            var groupByDate = from r in records
                              group r by r.DateOfBusiness into g
                              select new { DateOfBusiness = g.Key, SOS = g };

            var averages = new List<decimal>();
            foreach (var group in groupByDate)
            {
                var avg = Convert.ToDecimal(group.SOS.Average(r => r.OrderLastBumpTime)) / 60m;
                averages.Add(avg);
            }

            return averages.Average();
        }

        public static async Task<ICollection<SOSAverageTT>> GetAverageTTAsync(this DbSet<SOSAverageTT> set, Guid store, DateTime startDate, DateTime endDate)
        {
            var records = await (from sos in set
                                 where sos.StoreId == store && sos.BusinessDate >= startDate && sos.BusinessDate <= endDate
                                 select sos).ToListAsync();

            return records;
        }

        public static async Task<ICollection<SOSSummary>> GetSummaries(this DbSet<SOSSummary> set, Guid store, DateTime startDate, DateTime endDate)
        {
            var records = await (from sos in set
                                 where sos.StoreId == store && sos.BusinessDate >= startDate && sos.BusinessDate <= endDate
                                 select sos).ToListAsync();
            return records;
        }
    }
}
