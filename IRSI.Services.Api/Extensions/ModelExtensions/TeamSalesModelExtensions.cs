using IRSI.Services.Api.Models.Data.TeamSales.Aloha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Extensions.ModelExtensions
{
    public static class TeamSalesModelExtensions
    {
        public static string GetFullName(this EmployeeByStore employee)
        {
            var fullName = $"{employee.LastName.ToLower()}, {employee.FirstName.ToLower()}";
            return fullName;
        }
    }
}
