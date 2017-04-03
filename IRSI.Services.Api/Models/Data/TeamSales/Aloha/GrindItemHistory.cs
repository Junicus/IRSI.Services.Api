using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.Data.TeamSales.Aloha
{
    [Table(name: "dpvHstGndItem")]
    public class GrindItemHistory
    {
        [Key]
        [Column(name: "UniqueID")]
        public int Id { get; set; }

        [Column(name: "DateOfBusiness")]
        public DateTime DateOfBusiness { get; set; }

        [ForeignKey(name: "Store")]
        [Column(name: "FKStoreId")]
        public int StoreId { get; set; }
        public Store Store { get; set; }


        [Column(name: "FKEmployeeNumber")]
        public int? EmployeeNumber { get; set; }

        [Column(name: "CheckNumber")]
        public int? CheckNumber { get; set; }

        [ForeignKey("Item")]
        [Column(name: "FKItemId")]
        public int? ItemId { get; set; }
        public Item Item { get; set; }

        [ForeignKey("ParentItem")]
        [Column(name: "ParentId")]
        public int? ParentItemId { get; set; }
        public Item ParentItem { get; set; }

        [ForeignKey("Category")]
        [Column(name: "FKCategoryId")]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        [Column(name: "FKOrderModeId")]
        public int? OrderModeId { get; set; }


        [Column(name: "FKDayPartId")]
        public int? DayPartId { get; set; }

        [Column(name: "SystemDate")]
        public DateTime? SystemDate { get; set; }

        [Column(name: "Price")]
        public decimal? Price { get; set; }

        [Column(name: "DiscPric")]
        public decimal? DiscountPrice { get; set; }

        [Column(name: "Quantity")]
        public int? Quantity { get; set; }
    }
}
