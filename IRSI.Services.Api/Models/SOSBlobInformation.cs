using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Models
{
    public class SOSBlobInformation
    {
        public Uri BlobUrl { get; set; }
        public Guid StoreId { get; set; }
        public Guid RegionId { get; set; }
        public Guid ConceptId { get; set; }
        public DateTime BusinessDate { get; set; }

        public string BlobName
        {
            get
            {
                return BlobUrl.Segments[BlobUrl.Segments.Count() - 1];
            }
        }
    }
}
