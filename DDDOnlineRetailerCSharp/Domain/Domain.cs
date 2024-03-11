namespace DDDOnlineRetailerCSharp.Domain;

public class OutOfStock : Exception
{
    public OutOfStock()
    {
    }

    public OutOfStock(string message)
        : base(message)
    {
    }

    public OutOfStock(string message, Exception inner)
        : base(message, inner)
    {
    }
}
public class Domain
{
    public static string Allocate(OrderLine line, IEnumerable<Batch> batches)
    {
        Batch? batch = batches.Order().FirstOrDefault(b => b.CanAllocate(line));
        
        if (batch == null)
        {
            Console.WriteLine("batch is null");
            throw new OutOfStock($"Out of stock for sku {line.Sku}");
        }
        else
        {
            Console.WriteLine("batch is not null");
            Console.WriteLine(batch);
        }
        batch.Allocate(line);
        return batch.Reference!;
    }
}