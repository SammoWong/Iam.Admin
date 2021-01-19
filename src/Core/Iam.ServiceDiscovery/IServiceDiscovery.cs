using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.ServiceDiscovery
{
    public interface IServiceDiscovery
    {
        Task<IEnumerable<ServiceInformation>> GetServicesAsync(string serviceName);
    }
}
