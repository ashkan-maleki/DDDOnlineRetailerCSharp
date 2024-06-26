using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.EventBus;

namespace DDDOnlineRetailerCSharp.Link.Services.IntegrationEvents;

public interface IIntegrationEventBus
{
    void RegisterHandler<T>(Func<T, Task> handler) where T : Event;
    Task HandleAsync(Event @event);
}