using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iam.Identity.ServiceHost.Services
{
    public class JwtToken
    {
        public DateTime Expiration { get; set; }

        public string AccessToken { get; set; }

        public string TokenType { get; set; }
    }
}
