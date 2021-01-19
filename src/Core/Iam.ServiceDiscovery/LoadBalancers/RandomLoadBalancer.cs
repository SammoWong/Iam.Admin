using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.ServiceDiscovery.LoadBalancers
{
    /// <summary>
    /// 随机选取策略
    /// </summary>
    public class RandomLoadBalancer : ILoadBalancer
    {
        private readonly Random _random = new Random();
        public ServiceInformation Load(IList<ServiceInformation> services)
        {
            var index = _random.Next(services.Count());
            return services[index];
        }
    }
}
