using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.ServiceDiscovery.Consul
{
    internal class ConsulClientFactory
    {
        public static IConsulClient Create(Uri address)
        {
            return new ConsulClient(p =>
            {
                p.Address = address;
            });
        }
    }
}
