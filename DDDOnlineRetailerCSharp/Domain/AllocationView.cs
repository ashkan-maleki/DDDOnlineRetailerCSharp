namespace DDDOnlineRetailerCSharp.Domain;

public class AllocationView(string sku, string orderId, string batchRef)
{
    public string? Sku { get; } = sku;
    public string? OrderId { get; } = orderId;
    public string? BatchRef { get; } = batchRef;
}