using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Api.Configuration
{
    public class TeamSalesDbContextFactoryOptions
    {
        public TeamSalesDbContextFactoryOptions()
        {
            ConnectionString = new Dictionary<string, string>();
        }

        public IDictionary<string, string> ConnectionString { get; set; }
    }
}
