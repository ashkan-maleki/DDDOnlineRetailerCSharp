using DDDOnlineRetailerCSharp.EventBus;

namespace DDDOnlineRetailerCSharp.Domain;




public record DomainEvent : Event;

public record OutOfStockDomainEvent(string Sku) : DomainEvent
{
    public string Sku { get; } = Sku;
}

public record BatchCreated(string Reference, string Sku, int Qty, DateTime? Eta = null) : Event;

public record BatchQuantityChanged(string Reference, int Qty) : Event;

public record Deallocated(string OrderId, string Sku, int Qty) : DomainEvent;