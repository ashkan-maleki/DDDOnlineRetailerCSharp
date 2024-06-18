using DDDOnlineRetailerCSharp.EventBus;

namespace DDDOnlineRetailerCSharp.Domain;




public record DomainEvent : Event;

public record OutOfStockDomainEvent(string Sku) : DomainEvent
{
    public string Sku { get; } = Sku;
}

public record BatchCreatedDomainEvent(string Reference, string Sku, int Qty, DateTime? Eta = null) : DomainEvent;

public record BatchQuantityChangedDomainEvent(string Reference, int Qty) : DomainEvent;

public record DeallocatedDomainEvent(string OrderId, string Sku, int Qty) : DomainEvent;

public record AllocatedDomainEvent(string OrderId, string Sku, int Qty, string Reference) : DomainEvent;