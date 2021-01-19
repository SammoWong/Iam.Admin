using Iam.ServiceDiscovery.LoadBalancers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.ServiceDiscovery.Consul
{
    public class ConsulServiceSelector : IServiceSelector
    {
        public ServiceInformation Select(IEnumerable<ServiceInformation> services, ILoadBalancer loadBalancer)
        {
            return loadBalancer.Load(services.ToList());
        }
    }
}
