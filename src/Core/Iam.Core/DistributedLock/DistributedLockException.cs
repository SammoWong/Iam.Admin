using System;

namespace Iam.Core.DistributedLock
{
    public class DistributedLockException : Exception
    {
        /// <summary>
        ///  锁定资源id
        /// </summary>
        public string Id { set; get; }

        /// <summary>
        ///  单位毫秒
        /// </summary>
        public int ExpiredTime { get; set; }

        /// <summary>
        ///  单位毫秒
        /// </summary>
        public int WaitTime { get; set; }

        /// <summary>
        ///  单位毫秒
        /// </summary>
        public int RetryTime { get; set; }

        public DistributedLockException(string id, int expiredTime, int waitTime, int retryTime)
            : base($"lock fail->resource id:{id},expiredTime:{expiredTime},waitTime:{waitTime},retryTime:{retryTime}")
        {
            Id = id;
            ExpiredTime = expiredTime;
            WaitTime = waitTime;
            RetryTime = retryTime;
        }
    }
}
