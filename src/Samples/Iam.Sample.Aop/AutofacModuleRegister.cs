using Autofac;
using Autofac.Extras.DynamicProxy;
using Iam.Aop.Castle.Core.Interceptors;
using Iam.Core.Aop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Iam.Sample.Aop
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注册拦截器CachingInterceptor,ElapsedTimerInterceptor
            builder.RegisterType<CachingInterceptor>();
            builder.RegisterType<ElapsedTimerInterceptor>();

            //拦截器的执行顺序与添加顺序有关，先添加的先执行
            List<Type> interceptorTypes = new List<Type>() 
            { 
                typeof(ElapsedTimerInterceptor),
                typeof(CachingInterceptor),
            };

            //方式1：实现IAopService，启用interface代理拦截
            builder.RegisterTypes(typeof(CachingService)).As<IAopService>()
               .AsImplementedInterfaces()
               .EnableInterfaceInterceptors()//对目标类型启用接口拦截
               .InterceptedBy(typeof(CachingInterceptor));


            //方式2：当前程序集中启动多个拦截器功能
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var selectedAssemblies = allAssemblies.Where(a => a.FullName.Contains("Sample")).ToList();

            //builder.RegisterAssemblyTypes(selectedAssemblies.ToArray())
            //    .AsImplementedInterfaces()
            //    .InstancePerDependency()
            //    .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
            //    .InterceptedBy(interceptorTypes.ToArray());//允许将拦截器服务的列表分配给注册。


            //方式3：没有接口层的服务TestService，其方法需要设置为virtual
            //builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(TestService)))
            //    .EnableClassInterceptors()
            //    .InterceptedBy(interceptorTypes.ToArray());
        }
    }
}
