using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.ServiceDiscovery.Consul
{
    public class ConsulServiceDiscovery : IServiceDiscovery
    {
        private readonly ConsulClient _consul;

        public ConsulServiceDiscovery(ConsulClient consul)
        {
            _consul = consul;
        }

        public async Task<IEnumerable<ServiceInformation>> GetServicesAsync(string serviceName)
        {
            var queryResult = await _consul.Health.Service(serviceName);

            var services = queryResult.Response.Select(serviceEntry => new ServiceInformation
            {
                Name = serviceEntry.Service.Service,
                Id = serviceEntry.Service.ID,
                Host = serviceEntry.Service.Address,
                Port = serviceEntry.Service.Port,
                Tags = serviceEntry.Service.Tags
            });
            return services;
        }
    }
}
