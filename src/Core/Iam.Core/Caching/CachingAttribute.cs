using System;

namespace Iam.Core.Caching
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CachingAttribute : Attribute
    {
        public CachingAttribute(string cacheKey)
        {
            CacheKey = cacheKey;
        }

        public CachingAttribute(string cacheKey, TimeSpan expiration)
        {
            CacheKey = cacheKey;
            Expiration = expiration;
        }

        public string CacheKey { get; set; }

        /// <summary>
        /// 缓存过期时间
        /// </summary>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromHours(1);
    }
}
