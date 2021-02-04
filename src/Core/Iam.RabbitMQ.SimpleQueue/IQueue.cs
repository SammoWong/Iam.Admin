using System;

namespace Iam.RabbitMQ.SimpleQueue
{
    public interface IQueue<T>
    {
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="obj"></param>
        void Publish(T obj);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="action"></param>
        void Subscribe(Action<T> action);
    }
}
