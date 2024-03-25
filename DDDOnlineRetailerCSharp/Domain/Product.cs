namespace DDDOnlineRetailerCSharp.Domain;

public class Product(string sku, ICollection<Batch> batches, int versionNumber = 0)
{
    private Product(string sku, int versionNumber = 0) : this(sku, new List<Batch>(), versionNumber)
    {
        
    }
    public string Sku { get; } = sku;

    public int VersionNumber { get; private set; } = versionNumber;

    public ICollection<Batch> Batches { get; } = batches;

    public string Allocate(OrderLine line)
    {
        Batch? batch = Batches.Order().FirstOrDefault(b => b.CanAllocate(line));
        if (batch == null)
        {
            throw new OutOfStock($"Out of stock for sku {line.Sku}");
        }
        batch.Allocate(line);
        VersionNumber++;
        return batch.Reference!;
    }
}