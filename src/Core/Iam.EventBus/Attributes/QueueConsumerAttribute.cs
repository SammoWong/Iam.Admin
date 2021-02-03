using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.EventBus.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class QueueConsumerAttribute : Attribute
    {
        public string QueueName { get; private set; }

        public ushort PrefetchCount { get; private set; }

        public QueueConsumerAttribute(string queueName)
        {
            QueueName = queueName;
        }

        public QueueConsumerAttribute(string queueName, ushort prefetchCount = 1)
        {
            QueueName = queueName;
            PrefetchCount = prefetchCount;
        }
    }
}
