using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.Data.TeamSales
{
    public class CategoryGroupSalesItem
    {
        public int StoreId { get; set; }
        public int EmployeeId { get; set; }
        public decimal CategoryGroupSales { get; set; }
    }
}
