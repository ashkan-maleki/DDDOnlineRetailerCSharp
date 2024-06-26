using DDDOnlineRetailerCSharp.Application;

namespace DDDOnlineRetailerCSharp.Link.Services.IntegrationEvents;

public static class IntegrationEventBusFactory
{
    public static IntegrationEventBus RegisterAll(IIntegrationEventHandler handler)
    {
        IntegrationEventBus integrationEventBus = new();
        integrationEventBus.RegisterHandler<OutOfStockIntegrationEvent>(handler.HandleAsync);
        integrationEventBus.RegisterHandler<AllocatedIntegrationEvent>(handler.HandleAsync);
        integrationEventBus.RegisterHandler<DeallocatedIntegrationEvent>(handler.HandleAsync);
        return integrationEventBus;
    }
}