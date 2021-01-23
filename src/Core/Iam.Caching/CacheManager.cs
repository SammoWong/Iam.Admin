using EasyCaching.Core;
using Iam.Core.Caching;
using System;

namespace Iam.Caching
{
    public class CacheManager : ICache
    {
        private readonly IEasyCachingProvider _provider;

        public CacheManager(IEasyCachingProvider provider)
        {
            _provider = provider;
        }

        public void Clear()
        {
            _provider.Flush();
        }

        public bool Exists(string cacheKey)
        {
            return _provider.Exists(cacheKey);
        }

        public T Get<T>(string cacheKey)
        {
            return _provider.Get<T>(cacheKey).Value;
        }

        public void Remove(string cacheKey)
        {
            _provider.Remove(cacheKey);
        }

        public void Set<T>(string cacheKey, T value, TimeSpan? expiration = null)
        {
            expiration = expiration ?? TimeSpan.FromDays(1);
            _provider.Set(cacheKey, value, expiration.GetValueOrDefault());
        }

        public bool TrySet<T>(string cacheKey, T value, TimeSpan? expiration = null)
        {
            expiration = expiration ?? TimeSpan.FromDays(1);
            return _provider.TrySet(cacheKey, value, expiration.GetValueOrDefault());
        }
    }
}
