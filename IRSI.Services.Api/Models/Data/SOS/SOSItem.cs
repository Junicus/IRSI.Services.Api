using IRSI.Services.Models.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.Data.SOS
{
    public class SOSItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }
        public Guid ConceptId { get; set; }
        public Guid RegionId { get; set; }
        public Guid SOSFileId { get; set; }

        public DateTime DateOfBusiness { get; set; }

        public long TransactionNumber { get; set; }
        public int Course { get; set; }
        public int TerminalNumber { get; set; }
        public int Destination { get; set; }
        public int VirtualDisplayId { get; set; }
        public int CurrentActivityLevel { get; set; }
        public int DisplayGroupId { get; set; }
        public int ItemId { get; set; }
        public int Modifier1Id { get; set; }
        public int Modifier2Id { get; set; }
        public int Modifier3Id { get; set; }
        public DateTime OrderStartTime { get; set; }
        public int FirstStoreTime { get; set; }
        public int LastTotalTime { get; set; }
        public int LastRecallTime { get; set; }
        public int OrderPaidTime { get; set; }
        public int OrderFirstDisplayedTime { get; set; }
        public int OrderParkTime { get; set; }
        public int OrderLastBumpTime { get; set; }
        public string SOSTag { get; set; }
        public string ItemDescription { get; set; }
        public int ItemQantity { get; set; }
        public string DestinationName { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }

        [ForeignKey("ConceptId")]
        public virtual Concept Concept { get; set; }

        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }

        [ForeignKey("SOSFileId")]
        public virtual SOSFile SOSFile { get; set; }

        public virtual ICollection<SOSSummaryItems> SOSSummaryItems { get; set; }
    }
}
