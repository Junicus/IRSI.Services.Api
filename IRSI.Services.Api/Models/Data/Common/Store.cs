using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Models.Data.Common
{
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }

        public Guid ConceptId { get; set; }
        public Guid RegionId { get; set; }

        [ForeignKey("ConceptId")]
        public Concept Concept { get; set; }

        [ForeignKey("RegionId")]
        public Region Region { get; set; }
    }
}
