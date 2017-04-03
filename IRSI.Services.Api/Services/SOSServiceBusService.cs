using IRSI.Services.Api.Configuration.SOSServiceBusService;
using IRSI.Services.Api.Models;
using IRSI.Services.Api.Models.Data.SOS;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Services
{
    public class SOSServiceBusService : ISOSServiceBusService
    {
        private SOSServiceBusServiceOptions _options;
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;
        private CloudQueueClient _queueClient;
        private ILogger<SOSServiceBusService> _log;

        public SOSServiceBusService(IOptions<SOSServiceBusServiceOptions> options, ILogger<SOSServiceBusService> loggerFactory)
        {
            _options = options.Value;
            _storageAccount = CloudStorageAccount.Parse(_options.StorageConnectionString);
            _blobClient = _storageAccount.CreateCloudBlobClient();
            _queueClient = _storageAccount.CreateCloudQueueClient();
            _log = loggerFactory;
        }

        public async Task PostSOSDataAsync(IList<SOSItem> sosItems)
        {
            try
            {
                var sosContainer = _blobClient.GetContainerReference("sosdata");
                var sosQueue = _queueClient.GetQueueReference("sosqueue");

                var dataJson = JsonConvert.SerializeObject(sosItems);
                string blobName = $"{sosItems.First().StoreId}-{Guid.NewGuid().ToString()}";
                var dataBlob = sosContainer.GetBlockBlobReference(blobName);
                await dataBlob.UploadTextAsync(dataJson);

                var blobInfo = new SOSBlobInformation()
                {
                    StoreId = sosItems.First().StoreId,
                    RegionId = sosItems.First().RegionId,
                    ConceptId = sosItems.First().ConceptId,
                    BusinessDate = sosItems.First().DateOfBusiness,
                    BlobUrl = dataBlob.Uri
                };
                var queueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(blobInfo));
                await sosQueue.AddMessageAsync(queueMessage);
            }
            catch (Exception ex)
            {
                _log.LogError("Error posting to azure storage", ex);
                throw ex;
            }
        }
    }
}
