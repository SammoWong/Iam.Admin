using Iam.EventBus.Abstractions;
using Iam.EventBus.Attributes;
using Iam.EventBus.Events;
using Iam.EventBus.RabbitMQ.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Iam.EventBus.RabbitMQ
{
    /// <summary>
    /// 基于RabbitMQ的事件总线
    /// </summary>
    public class RabbitMQEventBus : IEventBus, IDisposable
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<RabbitMQEventBus> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly RabbitMQEventBusOptions _options;
        private readonly string _exchangeName;
        private readonly int _retryCount;
        private readonly ushort _prefetchCount;
        private readonly string _queueName;
        private IDictionary<string, IModel> _consumerChannels;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMQEventBus(IRabbitMQPersistentConnection persistentConnection, ILogger<RabbitMQEventBus> logger,
            IEventBusSubscriptionsManager subsManager, IOptions<RabbitMQEventBusOptions> options, 
            IServiceProvider serviceProvider)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger;
            _subsManager = subsManager ?? throw new ArgumentNullException(nameof(subsManager));
            _options = options.Value;
            _exchangeName = _options.ExchangeName;
            _retryCount = _options.RetryCount;
            _prefetchCount = _options.PrefetchCount;
            _queueName = _options.QueueName;
            _consumerChannels = new Dictionary<string, IModel>();
            _serviceProvider = serviceProvider;
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="event"></param>
        public void Publish(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    Console.WriteLine($"Could not publish event: {@event.Id} after {time.TotalSeconds:n1}s ({ex.Message})");
                });

            var eventName = @event.GetType().Name;
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: _exchangeName, type: "direct");

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    channel.BasicPublish(exchange: _exchangeName, routingKey: eventName, mandatory: true, basicProperties: properties, body: body);
                });
            }
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            var (QueueName, PrefetchCount) = ConvertConsumerInfo<TH>();

            DoInternalSubscription(eventName, QueueName, PrefetchCount);

            _subsManager.AddSubscription<T, TH>();
        }

        /// <summary>
        /// 动态订阅
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            var (QueueName, PrefetchCount) = ConvertConsumerInfo<TH>();

            DoInternalSubscription(eventName, QueueName, PrefetchCount);

            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _subsManager.RemoveSubscription<T, TH>();
        }

        /// <summary>
        /// 取消动态订阅
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Dispose()
        {
            foreach (var key in _consumerChannels.Keys)
            {
                if (_consumerChannels.TryGetValue(key, out var _consumerChannel) && _consumerChannel != null)
                {
                    _consumerChannel.Dispose();
                }
            }
            _subsManager.Clear();
        }

        public void StartSubscribe()
        {
            foreach (var queueName in _consumerChannels.Keys)
            {
                _consumerChannels.TryGetValue(queueName, out var _consumerChannel);

                if (_consumerChannel == null || _consumerChannel.IsClosed)
                    _consumerChannel = CreateConsumerChannel(queueName, _prefetchCount);

                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += async (model, ea) =>
                {
                    var eventName = ea.RoutingKey;
                    var message = Encoding.UTF8.GetString(ea.Body.Span);

                    //try
                    //{
                    await ProcessEvent(eventName, message);
                    //}
                    //catch (Exception exception)
                    //{
                    //    StringBuilder sb = new StringBuilder();
                    //    sb.Append($"【异常时间】：{DnsHelper.GetLanIp()}|{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\r\n");
                    //    sb.Append($"【队列消息】：{message}\r\n");
                    //    sb.Append($"【异常类型】：{exception.GetType().Name}\r\n");
                    //    sb.Append($"【异常信息】：{exception.Message}\r\n");
                    //    var sw = System.IO.File.AppendText($"{AppDomain.CurrentDomain.BaseDirectory}\\eventBus\\{eventName}.log");
                    //    sw.WriteLine(sb.ToString());
                    //    sw.Close();
                    //}
                    _consumerChannel.BasicAck(ea.DeliveryTag, multiple: false);
                };

                _consumerChannel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            }
        }

        /// <summary>
        /// 转换事件处理器队列名
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <returns></returns>
        private (string QueueName, ushort PrefetchCount) ConvertConsumerInfo<TH>()
        {
            var queueName = _queueName;
            var prefetchCount = _prefetchCount;
            var consumerAttr = typeof(TH).GetCustomAttribute<QueueConsumerAttribute>();
            if (consumerAttr != null)
            {
                queueName = consumerAttr.QueueName;
                prefetchCount = consumerAttr.PrefetchCount <= 0 ? prefetchCount : consumerAttr.PrefetchCount;
            }
            return (queueName, prefetchCount);
        }

        private void DoInternalSubscription(string eventName, string queueName, ushort prefetchCount)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                if (!_consumerChannels.ContainsKey(queueName))
                {
                    _consumerChannels.Add(queueName, CreateConsumerChannel(queueName, prefetchCount));
                }

                using (var channel = _persistentConnection.CreateModel())
                {
                    channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: eventName, arguments: new Dictionary<string, object> { { "x-queue-mode", "lazy" } });
                }
            }
        }

        /// <summary>
        /// 创建消费监听
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="prefetchCount"></param>
        /// <returns></returns>
        private IModel CreateConsumerChannel(string queueName, ushort prefetchCount)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchangeName, type: "direct");

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object> { { "x-queue-mode", "lazy" } });

            channel.BasicQos(0, prefetchCount, false);

            channel.CallbackException += (sender, ea) =>
            {
                channel.Dispose();
                channel = CreateConsumerChannel(queueName, prefetchCount);
            };

            return channel;
        }

        /// <summary>
        /// RabbitMQ取消订阅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventName"></param>
        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                foreach (var queueName in _consumerChannels.Keys)
                {
                    channel.QueueUnbind(queue: queueName,
                                        exchange: _exchangeName,
                                        routingKey: eventName);
                }
            }
        }

        /// <summary>
        /// 执行器
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        {
                            if (!(scope.ServiceProvider.GetRequiredService(subscription.HandlerType) is IDynamicIntegrationEventHandler handler)) continue;

                            await Task.Yield();
                            await handler.Handle(message);
                        }
                        else
                        {
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            if (eventType == null) continue;
                            var handler = scope.ServiceProvider.GetRequiredService(subscription.HandlerType);
                            if (handler == null) continue;
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                            await Task.Yield();
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
            }
        }
    }
}
