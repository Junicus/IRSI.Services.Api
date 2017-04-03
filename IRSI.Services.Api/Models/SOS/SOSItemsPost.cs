using IRSI.Services.Api.Models.Data.SOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models.SOS
{
    public class SOSItemsPost
    {
        public Guid StoreId { get; set; }
        public string Filename { get; set; }
        public DateTime BusinessDate { get; set; }
        public IList<SOSItem> SOSItems { get; set; }
    }
}
