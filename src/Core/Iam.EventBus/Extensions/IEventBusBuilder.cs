using Iam.EventBus.Abstractions;
using Iam.EventBus.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.EventBus.Extensions
{
    public interface IEventBusBuilder
    {
        IServiceCollection Services { get; }

        IConfiguration Configuration { get; }
    }

    public class EventBusBuilder : IEventBusBuilder
    {
        public IServiceCollection Services { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public EventBusBuilder(IServiceCollection services, IConfiguration configurationRoot)
        {
            Configuration = configurationRoot;
            Services = services;

            Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}
