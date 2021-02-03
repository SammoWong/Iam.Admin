using Iam.EventBus.Abstractions;
using Iam.EventBus.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iam.Sample.EventBus.EventHandlers
{
    //[QueueConsumer("custom_queue", 3)]//自定义queue
    public class MessageReceivedIntegrationEventHandler : IIntegrationEventHandler<MessageReceivedIntegrationEvent>
    {
        public Task Handle(MessageReceivedIntegrationEvent @event)
        {
            Console.WriteLine(@event.Message);
            return Task.CompletedTask;
        }
    }
}
