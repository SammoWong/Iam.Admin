using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.ServiceDiscovery
{
    public class ServiceInformation
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public Uri ToUri(string scheme = "http") => new Uri($"{scheme}://" + Host + ":" + Port);
        
    }
}
