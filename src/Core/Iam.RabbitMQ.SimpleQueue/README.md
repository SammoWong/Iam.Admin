## Example

### Message

```cs
public sealed record Product(string Name);
```

### Queue

```cs
public interface IProductQueue : IQueue<Product> { }
```

```cs
public class ProductQueue : Queue<Product>, IProductQueue
{
    public ProductQueue(Connection connection) : base(connection) { }
}
```

### Publisher

```cs
var product = new Product("Product");

IProductQueue productQueue = new ProductQueue(new Connection("localhost", 5672, "admin", "P4ssW0rd!"));

productQueue.Publish(product);
```

### Subscriber

```cs
IProductQueue productQueue = new ProductQueue(new Connection("localhost", 5672, "admin", "P4ssW0rd!"));

productQueue.Subscribe(product => Handle(product));
```
