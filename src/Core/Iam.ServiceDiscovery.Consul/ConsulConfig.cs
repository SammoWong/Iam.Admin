using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.ServiceDiscovery.Consul
{
    public class ConsulConfig
    {
        public string ConsulUrl { get; set; }
        public string ServiceName { get; set; }
        public string ServiceUrl { get; set; }
        public string HealthCheckUrl { get; set; }
        public int HealthCheckIntervalInSecond { get; set; }
        public string[] ServiceTags { get; set; }
        public string ConsulKeyPath { get; set; }
    }
}
