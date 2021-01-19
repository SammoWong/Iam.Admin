using Iam.ServiceDiscovery.LoadBalancers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.ServiceDiscovery
{
    public interface IServiceSelector
    {
        ServiceInformation Select(IEnumerable<ServiceInformation> services, ILoadBalancer loadBalancer);
    }
}
