using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.Data.TeamSales.Aloha
{
    [Table(name: "dpvHstShift")]
    public class ShiftHistory
    {
        [Key]
        [Column(name: "UniqueId")]
        public int Id { get; set; }

        [Column(name: "DateOfBusiness")]
        public DateTime DateOfBusiness { get; set; }

        [ForeignKey(name: "Store")]
        [Column(name: "FKStoreId")]
        public int StoreId { get; set; }
        public Store Store { get; set; }


        [Column(name: "EmployeeShiftNumber")]
        public int ShiftNumber { get; set; }

        [Column(name: "FKEmployeeNumber")]
        public int EmployeeNumber { get; set; }

        [Column(name: "FKJobCodeId")]
        public int JobCodeId { get; set; }

        [Column(name: "InHour")]
        public int InHour { get; set; }

        [Column(name: "InMinute")]
        public int InMinute { get; set; }

        [Column(name: "OutHour")]
        public int OutHour { get; set; }

        [Column(name: "OutMinute")]
        public int OutMinute { get; set; }

        [Column(name: "TotalMinutes")]
        public int TotalMinutes { get; set; }

        [Column(name: "TotalHours")]
        public double TotalHours { get; set; }

        [Column(name: "CCTips")]
        public double CreditCardTips { get; set; }

        [Column(name: "DeclaredTips")]
        public double DeclaredTips { get; set; }

        [Column(name: "Sales")]
        public double Sales { get; set; }

        [Column(name: "GROSSSLS")]
        public double Gross { get; set; }

        [Column(name: "SysDateIn")]
        public DateTime SystemDateIn { get; set; }

        [Column(name: "SysDateOut")]
        public DateTime SystemDateOut { get; set; }
    }
}
