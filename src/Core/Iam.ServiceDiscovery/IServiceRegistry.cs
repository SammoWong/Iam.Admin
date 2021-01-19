using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.ServiceDiscovery
{
    public interface IServiceRegistry
    {
        Task<ServiceInformation> RegisterServiceAsync(string serviceName, string version, string host, 
            int port, IEnumerable<string> tags = null);

        Task DeregisterServiceAsync(string serviceId);
    }
}
