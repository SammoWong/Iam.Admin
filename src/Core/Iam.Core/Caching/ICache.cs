using System;

namespace Iam.Core.Caching
{
    public interface ICache
    {
        bool Exists(string cacheKey);

        T Get<T>(string cacheKey);

        void Set<T>(string cacheKey, T value, TimeSpan? expiration = null);

        /// <summary>
        /// 当缓存数据不存在则添加，已存在不会添加，添加成功返回true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        bool TrySet<T>(string cacheKey, T value, TimeSpan? expiration = null);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        void Remove(string cacheKey);

        /// <summary>
        /// 清空缓存
        /// </summary>
        void Clear();
    }
}
