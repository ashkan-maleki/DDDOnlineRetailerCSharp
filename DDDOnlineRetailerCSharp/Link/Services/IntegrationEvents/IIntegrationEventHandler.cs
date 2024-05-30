using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.IntegrationEvents;

public interface IIntegrationEventHandler
{
    public Task HandleAsync(OutOfStock @event);
    public Task HandleAsync(BatchCreated @event);
    public Task HandleAsync(BatchQuantityChanged @event);
    public Task HandleAsync(Deallocated @event);
}