using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.Data.TeamSales.Aloha
{
    [Table(name: "EmployeeByStore")]
    public class EmployeeByStore
    {
        [Column(name: "EmployeeNumber", Order = 1)]
        public int EmployeeNumber { get; set; }

        [Column(name: "Owner", Order = 2)]
        public int StoreId { get; set; }

        [Column(name: "LastName")]
        public string LastName { get; set; }

        [Column(name: "FirstName")]
        public string FirstName { get; set; }

        [Column(name: "NickName")]
        public string NickName { get; set; }
    }
}
