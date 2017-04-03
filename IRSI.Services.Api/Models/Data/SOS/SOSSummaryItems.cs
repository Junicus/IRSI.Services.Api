using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.Data.SOS
{
    public class SOSSummaryItems
    {
        public Guid SOSSummaryId { get; set; }
        public Guid SOSItemId { get; set; }

        public virtual SOSItem Item { get; set; }
        public virtual SOSSummary Summary { get; set; }
    }
}
