using Castle.DynamicProxy;
using Iam.Core.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Iam.Aop.Castle.Core.Interceptors
{
    /// <summary>
    /// 缓存拦截器，一般此拦截器需要最后执行
    /// </summary>
    public class CachingInterceptor : IInterceptor
    {
        private readonly ICache _cache;

        public CachingInterceptor(ICache cache)
        {
            _cache = cache;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            var cachingAttr = method.GetCustomAttribute<CachingAttribute>();
            //无此特性或者无返回值或者返回值为Task时，跳过此拦截器
            if(cachingAttr == null || method.ReturnType == typeof(void) || method.ReturnType == typeof(Task))
            {
                invocation.Proceed();//执行下一个拦截器或者当前方法
            }
            else
            {
                //TODO：缓存的存取统一为string类型，再根据返回值做相应转换
                var cacheKey = cachingAttr.CacheKey;
                var expiration = cachingAttr.Expiration;
                
                var cacheValue = _cache.Get<string>(cacheKey);
                if(cacheValue != null)
                {
                    var returnType = typeof(Task).IsAssignableFrom(method.ReturnType) 
                        ? method.ReturnType.GenericTypeArguments.FirstOrDefault() 
                        : method.ReturnType;

                    dynamic result = JsonSerializer.Deserialize(cacheValue, returnType);
                    invocation.ReturnValue = (typeof(Task).IsAssignableFrom(method.ReturnType)) ? Task.FromResult(result) : result;
                    return;
                }
                //执行下一个拦截器或者当前方法
                invocation.Proceed();
                //存入缓存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    object response;
                    var type = invocation.Method.ReturnType;
                    if (typeof(Task).IsAssignableFrom(type))
                    {
                        var resultProperty = type.GetProperty("Result");
                        response = resultProperty.GetValue(invocation.ReturnValue);
                        type = resultProperty.PropertyType;
                    }
                    else
                    {
                        response = invocation.ReturnValue;
                    }
                    if (response == null) 
                        response = string.Empty;

                    var jsonValue = JsonSerializer.Serialize(response, type);
                    _cache.TrySet(cacheKey, jsonValue, expiration);
                }
            }
        }
    }
}
