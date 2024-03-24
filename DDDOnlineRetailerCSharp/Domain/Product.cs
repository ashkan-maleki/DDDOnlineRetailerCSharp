namespace DDDOnlineRetailerCSharp.Domain;

public class Product(string sku, IEnumerable<Batch> batches, int versionNumber = 0)
{
    public int Id { get; protected init; }
    public string Sku { get; } = sku;
    public int VersionNumber { get; private set; } = versionNumber;
    public IEnumerable<Batch> Batches { get; } = batches;

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