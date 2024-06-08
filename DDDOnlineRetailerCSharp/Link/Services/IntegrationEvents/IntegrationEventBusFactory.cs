using DDDOnlineRetailerCSharp.Application;
using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Services.DomainEvents;

namespace DDDOnlineRetailerCSharp.Link.Services.IntegrationEvents;

public static class IntegrationEventBusFactory
{
    public static IntegrationEventBus RegisterAll(IIntegrationEventHandler handler)
    {
        IntegrationEventBus integrationEventBus = new();
        integrationEventBus.RegisterHandler<OutOfStockIntegrationEvent>(handler.HandleAsync);
        return integrationEventBus;
    }
}