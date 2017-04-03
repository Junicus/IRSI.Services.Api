using IRSI.Services.Models.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.Data.SOS
{
    public class SOSFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }
        public DateTime BusinessDate { get; set; }
        public string Filename { get; set; }
        public DateTime DateAdded { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
    }
}
