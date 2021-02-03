using Iam.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.EventBus.Abstractions
{
    /// <summary>
    /// 事件总线接口
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="event">事件模型</param>
        void Publish(IntegrationEvent @event);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T">事件模型</typeparam>
        /// <typeparam name="TH">事件处理器<事件模型></typeparam>
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="T">事件模型</typeparam>
        /// <typeparam name="TH">事件处理器<事件模型></typeparam>
        void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// 动态订阅
        /// </summary>
        /// <typeparam name="TH">事件处理器</typeparam>
        /// <param name="eventName"></param>
        void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// 取消动态订阅
        /// </summary>
        /// <typeparam name="TH">事件处理器</typeparam>
        /// <param name="eventName"></param>
        void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        //TODO:FamilyBucket
        //void StartSubscribe();
    }
}
