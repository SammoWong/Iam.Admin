using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.ServiceDiscovery.Consul
{
    public class ConsulServiceRegistry : IServiceRegistry
    {
        private readonly IConsulClient _consul;

        public ConsulServiceRegistry(IConsulClient consul)
        {
            _consul = consul;
        }

        public async Task<ServiceInformation> RegisterServiceAsync(string serviceName, string serviceUrl, string healthCheckUrl, IEnumerable<string> tags = null)
        {
            var serviceUri = new Uri(serviceUrl);
            var serviceId = GetServiceId(serviceName, serviceUri.Host, serviceUri.Port);
            var scheme = serviceUri.Scheme;

            var registration = new AgentServiceRegistration
            {
                ID = serviceId,
                Name = serviceName,
                Tags = tags?.ToArray(),
                Address = serviceUri.Host,
                Port = serviceUri.Port,
                Check = new AgentCheckRegistration()
                {
                    HTTP = $"{scheme}://{serviceUri.Host}:{serviceUri.Port}{healthCheckUrl}",
                    //Status = HealthStatus.Passing,
                    Timeout = TimeSpan.FromSeconds(3),
                    Interval = TimeSpan.FromSeconds(10),
                    //服务启动多久后注册
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(3),
                }
            };

            await _consul.Agent.ServiceRegister(registration);

            return new ServiceInformation
            {
                Name = registration.Name,
                Id = registration.ID,
                Host = registration.Address,
                Port = registration.Port,
                Tags = tags
            };
        }

        public async Task DeregisterServiceAsync(string serviceId)
        {
            await _consul.Agent.ServiceDeregister(serviceId);
        }

        public string GetServiceId(string serviceName, string host, int port)
        {
            return $"{serviceName}.{host}.{port}";
        }
    }
}
