using EasyCaching.Core.Configurations;
using Iam.Core.Caching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Iam.Caching
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCache(this IServiceCollection services, Action<EasyCachingOptions> configAction)
        {
            services.TryAddScoped<ICache, CacheManager>();
            services.AddEasyCaching(configAction);
        }
    }
}
