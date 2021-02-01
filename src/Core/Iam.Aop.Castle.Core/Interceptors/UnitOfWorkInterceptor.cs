using Castle.DynamicProxy;
using Iam.Data.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace Iam.Aop.Castle.Core.Interceptors
{
    /// <summary>
    /// 工作单元拦截器
    /// </summary>
    public class UnitOfWorkInterceptor : IInterceptor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UnitOfWorkInterceptor> _logger;

        public UnitOfWorkInterceptor(IServiceProvider serviceProvider, ILogger<UnitOfWorkInterceptor> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            var uowAttr = method.GetCustomAttribute<UnitOfWorkAttribute>();
            if (uowAttr == null)
            {
                invocation.Proceed();//执行下一个拦截器或者当前方法
            }
            else
            {
                var unitOfWork = _serviceProvider.GetService<IUnitOfWork>();
                if(unitOfWork != null)
                {
                    unitOfWork.BeginTransaction();
                    try
                    {
                        invocation.Proceed();
                        unitOfWork.Commit();
                    }
                    catch(Exception ex)
                    {
                        unitOfWork.Rollback();
                        throw;
                    }
                }
                else
                {
                    _logger.LogError($"在拦截器中创建工作单元失败：{method.Name}");
                    invocation.Proceed();
                }
            }
        }
    }
}
