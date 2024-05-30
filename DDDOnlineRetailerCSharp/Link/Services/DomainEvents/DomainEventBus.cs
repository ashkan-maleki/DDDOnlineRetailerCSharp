using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.DomainEvents;

// IEventHandler

// EventHandler
public class DomainDomainEventBus(IUnitOfWork uow) : IDomainEventBus
{
    private readonly Dictionary<Type, Func<DomainEvent, Task>> _handlers = new();

    public void RegisterHandler<T>(Func<T, Task> handler) where T : DomainEvent
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            throw new IndexOutOfRangeException("You cannot register the same event handler twice");
        }

        _handlers.Add(typeof(T), x => handler((T)x));
    }

    public async Task HandleAsync(DomainEvent @event)
    {
        Queue<DomainEvent> events = new Queue<DomainEvent>(new[] { @event });
        while (events.Any())
        {
            @event = events.Dequeue();
            if (_handlers.TryGetValue(@event.GetType(), out Func<DomainEvent, Task>? handler))
            {
                await handler(@event);

                await foreach (var collectedEvent in uow.CollectNewEvents())
                {
                    events.Enqueue(collectedEvent.Result);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(handler), "No event handler was registered");
            }
        }
    }
}