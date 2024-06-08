using DDDOnlineRetailerCSharp.Application;
using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.IntegrationEvents;

public interface IIntegrationEventHandler
{
    public Task HandleAsync(OutOfStockIntegrationEvent @event);
}