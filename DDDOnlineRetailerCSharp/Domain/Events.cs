using System.Text.Json.Serialization;

namespace DDDOnlineRetailerCSharp.Domain;

public record Event
{
    [JsonInclude]
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonInclude]
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}

public record IntegrationEvent : Event;
public record DomainEvent : Event;

public record OutOfStock(string Sku) : DomainEvent
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

public record Deallocated(string OrderId, string Sku, int Qty) : DomainEvent
{
    public string OrderId { get; init; } = OrderId;
    public string Sku { get; init; } = Sku;
    public int Qty { get; init; } = Qty;
}