using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.TeamSales
{
    public class EmployeeSalesPerHourComparative
    {
        public int EmployeeId { get; set; }
        public int StoreId { get; set; }
        public string EmployeeName { get; set; }
        public double CurrentSalesPerHour { get; set; }
        public double ComparativeSalesPerHour { get; set; }
        public double Difference { get; set; }
    }
}
