using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Jwt
{
    public class JwtTokenBuilder
    {
        /// <summary>
        /// 生成基于JWT的Token
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static JwtToken BuildJwtToken(Claim[] claims, JwtConfig config)
        {
            var now = DateTime.Now;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecurityKey));
            //实例化JwtSecurityToken
            var jwt = new JwtSecurityToken(
                issuer: config.Issuer,
                audience: config.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(config.Expiration),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new JwtToken
            {
                AccessToken = encodedJwt,
                Expiration = now.AddMinutes(config.Expiration),
            };
        }
    }
}
