using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Jwt
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJwt(this IServiceCollection services, JwtConfig config)
        {
            //添加认证配置
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecurityKey)),
                    ValidateAudience = true,
                    ValidAudience = config.Audience,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    //ClockSkew = TimeSpan.FromMinutes(config.ClockSkew)//总的Token有效时间 = JwtRegisteredClaimNames.Exp + ClockSkew
                };
            });
        }
    }
}
