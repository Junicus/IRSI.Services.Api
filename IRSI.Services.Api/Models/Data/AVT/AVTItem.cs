using IRSI.Services.Api.Models.Data.TeamSales.Aloha;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.Data.AVT
{
    public class AVTItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string FiscalYear { get; set; }
        public int Period { get; set; }
        public Guid StoreId { get; set; }
        public decimal FGActual { get; set; }
        public decimal FGTarget { get; set; }
        public decimal AVTActual { get; set; }
        public decimal AVTTheoric { get; set; }
        public virtual Store Store { get; set; }
        public string ChampName { get; set; }
        public string ChampId { get; set; }
        public decimal PayTotal { get; set; }
    }
}
