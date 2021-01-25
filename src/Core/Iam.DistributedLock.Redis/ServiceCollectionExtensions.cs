using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.DistributedLock.Redis
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisDistributedLock(this IServiceCollection services, RedLockConfig config)
        {
            return services.AddSingleton(sp => { return new RedLockManager(config.Configuration); });
        }
    }
}
