namespace Iam.Sample.SimpleQueue
{
    public sealed record Product(string Name)
    {
        public override string ToString()
        {
            return $"ProductName is : {Name}";
        }
    }
}
