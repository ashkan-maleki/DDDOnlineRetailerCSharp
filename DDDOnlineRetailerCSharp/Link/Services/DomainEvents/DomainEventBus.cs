using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.DomainEvents;

// IEventHandler

// EventHandler
public class DomainDomainEventBus : IDomainEventBus
{
    private readonly Dictionary<Type, Func<DomainEvent, IUnitOfWork, Task>> _handlers = new();

    public void RegisterHandler<T>(Func<T, IUnitOfWork, Task> handler) where T : DomainEvent
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            throw new IndexOutOfRangeException("You cannot register the same event handler twice");
        }

        _handlers.Add(typeof(T), (x, uow) => handler((T)x, uow));
    }

    public async Task HandleAsync(DomainEvent @event, IUnitOfWork uow)
    {
        if (_handlers.TryGetValue(@event.GetType(), out Func<DomainEvent, IUnitOfWork, Task>? handler))
        {
            await handler(@event, uow);
        }
        else
        {
            throw new ArgumentNullException(nameof(handler), "No event handler was registered");
        }
    }
}