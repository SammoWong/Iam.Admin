using Iam.EventBus.Abstractions;
using Iam.EventBus.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.EventBus.RabbitMQ.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IEventBusBuilder UseRabbitMQ(this IEventBusBuilder builder, string sectionPath = "EventBus:RabbitMQ")
        {
            builder.Services.Configure<RabbitMQEventBusOptions>(builder.Configuration.GetSection(sectionPath));
            builder.Services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>();
            builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();
            return builder;
        }
    }
}
