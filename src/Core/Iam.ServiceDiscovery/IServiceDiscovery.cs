using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.ServiceDiscovery
{
    public interface IServiceDiscovery
    {
        /// <summary>
        /// 获取服务信息
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        Task<IEnumerable<ServiceInformation>> GetServicesAsync(string serviceName);
    }
}
