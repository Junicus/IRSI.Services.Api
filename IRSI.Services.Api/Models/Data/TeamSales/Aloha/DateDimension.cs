using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.Data.TeamSales.Aloha
{
    [Table(name: "DimDate")]
    public class DateDimension
    {
        [Key]
        [Column(name: "Date")]
        public DateTime Date { get; set; }

        [Column(name: "FiscalYear")]
        public double FiscalYear { get; set; }

        [Column(name: "FiscalPeriod")]
        public double FiscalPeriod { get; set; }

        [Column(name: "FiscalWeekOfPeriod")]
        public double FiscalWeekOfPeriod { get; set; }

        [Column(name: "FiscalDayOfPeriod")]
        public double FiscalDayOfPeriod { get; set; }
    }
}
