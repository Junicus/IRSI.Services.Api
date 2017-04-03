using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.Data.TeamSales.Aloha
{
    [Table(name: "Category")]
    public class Category
    {
        [Key]
        [Column(name: "CategoryId")]
        public int CategoryId { get; set; }

        [Column(name: "Name")]
        public string Name { get; set; }
    }
}
