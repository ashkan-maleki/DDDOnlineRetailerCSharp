using DDDOnlineRetailerCSharp.EventBus;

namespace DDDOnlineRetailerCSharp.Application;

public record IntegrationEvent : Event;


public record OutOfStockIntegrationEvent(string Sku) : IntegrationEvent
{
    public string Sku { get; } = Sku;
}

// RemoveAllocationFromViewIntegrationEvent
public record DeallocatedIntegrationEvent(string OrderId, string Sku, string Reference) : IntegrationEvent;


// AddAllocationToViewIntegrationEvent
public record AllocatedIntegrationEvent(string OrderId, string Sku, string Reference) : IntegrationEvent;