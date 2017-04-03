using IRSI.Services.Api.Models.Data.SOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Services
{
    public interface ISOSServiceBusService
    {
        Task PostSOSDataAsync(IList<SOSItem> sosItems);
    }
}
