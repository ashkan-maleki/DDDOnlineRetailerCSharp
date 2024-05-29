using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.Events;

public interface IEventBus
{
    void RegisterHandler<T>(Func<T, Task> handler) where T : Event;
    Task HandleAsync(Event @event);
}