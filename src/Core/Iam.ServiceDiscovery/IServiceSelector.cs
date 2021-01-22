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
        /// <summary>
        /// 选取服务节点
        /// </summary>
        /// <param name="services"></param>
        /// <param name="loadBalancer"></param>
        /// <returns></returns>
        ServiceInformation Select(IEnumerable<ServiceInformation> services, ILoadBalancer loadBalancer);
    }
}
