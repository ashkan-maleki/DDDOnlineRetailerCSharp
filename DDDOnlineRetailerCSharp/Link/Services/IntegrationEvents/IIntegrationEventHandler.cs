using DDDOnlineRetailerCSharp.Application;
using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.IntegrationEvents;

public interface IIntegrationEventHandler
{
    public Task HandleAsync(OutOfStockIntegrationEvent @event);
    public Task HandleAsync(DeallocatedIntegrationEvent @event);
    public Task HandleAsync(AllocatedIntegrationEvent @event);
}