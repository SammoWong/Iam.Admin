using Castle.DynamicProxy;
using Iam.Core.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Aop.Castle.Core.Interceptors
{
    /// <summary>
    /// 方法运行时间计时器，有多个拦截器时需要让此拦截器优先执行
    /// </summary>
    public class ElapsedTimerInterceptor : IInterceptor
    {
        private readonly ILogger<ElapsedTimerInterceptor> _logger;

        public ElapsedTimerInterceptor(ILogger<ElapsedTimerInterceptor> logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            var exist = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(ElapsedTimerAttribute)) is ElapsedTimerAttribute attr;
            var timerAttribute = method.GetCustomAttribute<ElapsedTimerAttribute>();
            if(timerAttribute == null)
            {
                invocation.Proceed();
            }
            else
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                invocation.Proceed();
                watch.Stop();
                var elapsedTime = watch.Elapsed.Milliseconds;
                var args = string.Join(',',invocation.Arguments?.ToList());
                var message = $"方法{method.Name}，参数{args}，运行时间{elapsedTime}毫秒";
                Console.WriteLine(message);
                _logger.LogInformation(message);
            }
        }
    }
}
