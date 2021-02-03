using RabbitMQ.Client;
using System;

namespace Iam.EventBus.RabbitMQ
{
    /// <summary>
    /// RabbitMQ持久连接接口
    /// </summary>
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
