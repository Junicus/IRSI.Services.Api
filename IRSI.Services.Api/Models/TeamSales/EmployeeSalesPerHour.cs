﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.TeamSales
{
    public class EmployeeSalesPerHour
    {
        public int EmployeeId { get; set; }
        public int StoreId { get; set; }
        public string EmployeeName { get; set; }
        public int Shifts { get; set; }
        public double SalesPerHour { get; set; }
    }
}
