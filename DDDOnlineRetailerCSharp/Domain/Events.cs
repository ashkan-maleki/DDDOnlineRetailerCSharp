using System.Text.Json.Serialization;

namespace DDDOnlineRetailerCSharp.Domain;

public record Event
{
    [JsonInclude]
    public Guid Id { get; set; }
    
    [JsonInclude]
    public DateTime CreationDate { get; set; }

    public Event()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }
    
}

public record OutOfStock(string Sku) : Event
{
    public string Sku { get; } = Sku;
}

public record BatchCreated(string Reference, string Sku, int Qty, DateTime? Eta = null) : Event
{
    public string Reference { get; init; } = Reference;
    public string Sku { get; init; } = Sku;
    public int Qty { get; init; } = Qty;
    public DateTime? Eta { get; init; } = Eta;
}

public record BatchQuantityChanged(string Reference, int Qty) : Event
{
    public string Reference { get; init; } = Reference;
    public int Qty { get; init; } = Qty;
}

public record AllocationRequired(string OrderId, string Sku, int Qty) : Event
{
    public string OrderId { get; init; } = OrderId;
    public string Sku { get; init; } = Sku;
    public int Qty { get; init; } = Qty;
}