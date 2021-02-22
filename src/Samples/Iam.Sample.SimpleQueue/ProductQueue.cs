using Iam.RabbitMQ.SimpleQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iam.Sample.SimpleQueue
{
    public class ProductQueue : RabbitMQ.SimpleQueue.Queue<Product>, IProductQueue
    {
        public ProductQueue(Connection connection) : base(connection) { }
    }
}
