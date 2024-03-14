namespace DDDOnlineRetailerCSharp.Domain;

public class OrderLine(string orderId, string sku, int qty) : ValueObject
{
    // private readonly List<Batch> _batches = new();
    public int Id { get; protected init; }
    public string OrderId { get; init; } = orderId;
    public string Sku { get; init; } = sku;
    public int Qty { get; init; } = qty;
    // public List<Batch> Batches => _batches;

    public void Deconstruct(out string OrderId, out string Sku, out int Qty)
    {
        OrderId = this.OrderId;
        Sku = this.Sku;
        Qty = this.Qty;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return OrderId;
        yield return Sku;
        yield return Qty;
    }

    // public override int GetHashCode()
    // {
    //     return GetEqualityComponents()
    //         .Select(x => x != null ? x.GetHashCode() : 0)
    //         .Aggregate((x, y) => x ^ y);
    // }
    // public override bool Equals(object obj)
    // {
    //     if (obj is OrderLine line)
    //     {
    //         return OrderId == line.OrderId
    //                && Sku == line.Sku
    //                && Qty == line.Qty;
    //     }
    //
    //     return false;
    // }
}