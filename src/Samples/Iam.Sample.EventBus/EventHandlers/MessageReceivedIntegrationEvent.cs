using Iam.EventBus.Attributes;
using Iam.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iam.Sample.EventBus.EventHandlers
{
    public class MessageReceivedIntegrationEvent : IntegrationEvent
    {
        public string Message { get; set; }
    }
}
