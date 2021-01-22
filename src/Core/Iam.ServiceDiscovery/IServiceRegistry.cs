using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.ServiceDiscovery
{
    public interface IServiceRegistry
    {
        string GetServiceId(string serviceName, string host, int port);

        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceUrl"></param>
        /// <param name="healthCheckUrl"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        Task<ServiceInformation> RegisterServiceAsync(string serviceName, string serviceUrl, string healthCheckUrl, IEnumerable<string> tags = null);

        Task DeregisterServiceAsync(string serviceId);
    }
}
