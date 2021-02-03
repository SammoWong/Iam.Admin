using Iam.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.EventBus.Abstractions
{
    /// <summary>
    /// 集成事件处理程序基接口
    /// </summary>
    public interface IIntegrationEventHandler
    {
    }

    /// <summary>
    /// 集成事件处理程序泛型接口
    /// </summary>
    /// <typeparam name="TIntegrationEvent"></typeparam>
    public interface IIntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandler 
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}
