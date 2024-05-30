using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.IntegrationEvents;

// IEventHandler

// EventHandler
public class IntegrationEventBus(IUnitOfWork uow) : IIntegrationEventBus
{
    private readonly Dictionary<Type, Func<Event, Task>> _handlers = new();

    public void RegisterHandler<T>(Func<T, Task> handler) where T : Event
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            throw new IndexOutOfRangeException("You cannot register the same event handler twice");
        }

        _handlers.Add(typeof(T), x => handler((T)x));
    }

    public async Task HandleAsync(Event @event)
    {
        Queue<Event> events = new Queue<Event>(new[] { @event });
        while (events.Any())
        {
            @event = events.Dequeue();
            if (_handlers.TryGetValue(@event.GetType(), out Func<Event, Task>? handler))
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