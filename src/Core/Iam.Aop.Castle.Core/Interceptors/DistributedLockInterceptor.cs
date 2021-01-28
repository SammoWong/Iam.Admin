using Castle.DynamicProxy;
using Iam.Core.DistributedLock;
using Iam.Core.DistributedLock.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Iam.Aop.Castle.Core.Interceptors
{
    public class DistributedLockInterceptor : IInterceptor
    {
        private readonly ILogger<DistributedLockInterceptor> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DistributedLockInterceptor(ILogger<DistributedLockInterceptor> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;

            //是否有此特性
            if (method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(DistributedLockAttribute)) is DistributedLockAttribute lockAttr)
            {
                if (AcquireLock(lockAttr))
                {
                    try
                    {
                        invocation.Proceed();
                    }
                    finally
                    {
                        ReleaseLock(lockAttr);
                    }
                }
                else
                {
                    var ex = new DistributedLockException(lockAttr.Id, lockAttr.ExpiredTime, lockAttr.ExpiredTime, lockAttr.RetryTime);
                    _logger.LogError($"获取分布式锁失败：{method.Name}", ex);
                    throw ex;
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
            }
        }

        /// <summary>
        /// 获取分布式锁
        /// </summary>
        /// <param name="lockAttr"></param>
        /// <returns></returns>
        private bool AcquireLock(DistributedLockAttribute lockAttr)
        {
            var lockManager = _serviceProvider.GetService<IDistributedLockManager>();
            bool acquireResult = false;
            if (lockAttr.RetryTime == default(int) || lockAttr.WaitTime == default(int))
            {
                acquireResult = lockManager.AcquireLock(lockAttr.Id, TimeSpan.FromMilliseconds(lockAttr.ExpiredTime));
            }
            else
            {
                acquireResult = lockManager.AcquireLock(lockAttr.Id,
                     TimeSpan.FromMilliseconds(lockAttr.ExpiredTime),
                     TimeSpan.FromMilliseconds(lockAttr.WaitTime),
                     TimeSpan.FromMilliseconds(lockAttr.RetryTime));
            }

            return acquireResult;
        }

        /// <summary>
        /// 释放分布式锁
        /// </summary>
        /// <param name="lockAttr"></param>
        private void ReleaseLock(DistributedLockAttribute lockAttr)
        {
            var lockManager = _serviceProvider.GetService<IDistributedLockManager>();
            var lockId = lockAttr.Id;
            lockManager.ReleaseLock(lockId);
        }
    }
}
