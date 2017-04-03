using IRSI.Services.Models.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.Data.SOS
{
    public class SOSSummary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }
        public Guid RegionId { get; set; }
        public Guid ConceptId { get; set; }
        public DateTime BusinessDate { get; set; }

        public Guid DayPartId { get; set; }

        public SummaryGroup SummaryGroup { get; set; }
        public int Count { get; set; }

        public virtual ICollection<SOSSummaryItems> SOSSummaryItems { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }

        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }

        [ForeignKey("ConceptId")]
        public virtual Concept Concept { get; set; }

        [ForeignKey("DayPartId")]
        public virtual DayPart DayPart { get; set; }
    }
}
