using Iam.RabbitMQ.SimpleQueue;
using System;

namespace Iam.Sample.SimpleQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            var product = new Product("Product");

            Publish(product);
            Subscribe();

            Console.ReadKey();
        }

        static void Publish(Product product)
        {
            IProductQueue productQueue = new ProductQueue(new Connection
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            });

            productQueue.Publish(product);
        }

        static void Subscribe()
        {
            IProductQueue productQueue = new ProductQueue(new Connection
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            });

            productQueue.Subscribe(product => Console.WriteLine(product.ToString()));
        }
    }
}
