using DDDOnlineRetailerCSharp.EventBus;

namespace DDDOnlineRetailerCSharp.Application;

public record IntegrationEvent : Event;


public record OutOfStockIntegrationEvent(string Sku) : IntegrationEvent
{
    public string Sku { get; } = Sku;
}