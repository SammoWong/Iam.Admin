using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Jwt
{
    public class JwtConfig
    {
        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 订阅人
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 过期时间，单位分钟
        /// </summary>
        public int Expiration { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string SecurityKey { get; set; }
    }
}
