using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.HealthCheck
{
    public static class ServiceCollectionExtensions
    {
        public static IHealthChecksBuilder AddHealthCheck(this IServiceCollection services)
        {
            return services.AddHealthChecks();
        }

        public static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints, string pattern = "/health", HealthCheckOptions options = null)
        {
            return endpoints.MapHealthChecks(pattern, options ?? new HealthCheckOptions()
            {
                Predicate = _ => true,
                // 该响应输出是一个json，包含所有检查项的详细检查结果
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}
