using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Core.Helpers
{
    public static class ServiceCollectionHelper
    {
        public static T GetService<T>(this IServiceCollection services)
        {
            var serviceDescriptor = services.FirstOrDefault(p => p.ServiceType == typeof(T))?.ImplementationFactory;
            return (T)serviceDescriptor.Invoke(null);
        }
    }
}
