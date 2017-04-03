using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.Data.TeamSales.Aloha
{
    [Table("Item")]
    public class Item
    {
        [Key]
        [Column(name: "ItemId")]
        public int ItemId { get; set; }

        [Column(name: "ShortName")]
        public string ShortName { get; set; }

        [Column(name: "ChitName")]
        public string ChitName { get; set; }

        [Column(name: "LongName")]
        public string LongName { get; set; }

        [Column(name: "Price")]
        public decimal? Price { get; set; }
    }
}
