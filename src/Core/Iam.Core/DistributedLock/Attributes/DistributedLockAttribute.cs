using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Core.DistributedLock.Attributes
{
    /// <summary>
    /// 分布式锁特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DistributedLockAttribute : Attribute
    {
        public DistributedLockAttribute()
        {

        }

        /// <summary>
        /// 获取锁失败则立即返回
        /// </summary>
        /// <param name="id">锁定的key值</param>
        /// <param name="expiredTime">key的过期时间</param>
        public DistributedLockAttribute(string id, int expiredTime)
        {
            Id = id;
            ExpiredTime = expiredTime;
        }

        /// <summary>
        /// 获取锁失败则重试
        /// </summary>
        /// <param name="id">锁定的key值</param>
        /// <param name="expiredTime">key的过期时间</param>
        /// <param name="waitTime">获取失败的阻塞时间</param>
        /// <param name="retryTime">获取失败的重试间隔</param>
        public DistributedLockAttribute(string id, int expiredTime, int waitTime, int retryTime)
        {
            Id = id;
            ExpiredTime = expiredTime;
            WaitTime = waitTime;
            RetryTime = retryTime;
        }

        /// <summary>
        ///  锁定资源id
        /// </summary>
        public string Id { set; get; }

        /// <summary>
        ///  过期时间
        ///  单位毫秒
        /// </summary>
        public int ExpiredTime { get; set; }

        /// <summary>
        ///  等待时间
        ///  单位毫秒
        /// </summary>
        public int WaitTime { get; set; }

        /// <summary>
        ///  重试间隔
        ///  单位毫秒
        /// </summary>
        public int RetryTime { get; set; }
    }
}
