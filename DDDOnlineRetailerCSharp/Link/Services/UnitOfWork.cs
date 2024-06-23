using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Adaptors;
using DDDOnlineRetailerCSharp.Link.Services.DomainEvents;
using DDDOnlineRetailerCSharp.Persistence;

namespace DDDOnlineRetailerCSharp.Link.Services;

public class UnitOfWork(RetailerDbContext dbContext, IRepository repository, IDomainEventBus eventBus) : IUnitOfWork
{
    public IRepository Repository { get; } = repository;
    public int Commit() => dbContext.SaveChanges();

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        int commit = await dbContext.SaveChangesAsync(cancellationToken);
        await DispatchDomainEventsAsync();
        return commit;
    }

    public async ValueTask DisposeAsync() => await dbContext.DisposeAsync();

    public async IAsyncEnumerable<Task<DomainEvent>> CollectNewEvents()
    {
        foreach (KeyValuePair<string, Product> pair in repository.Seen)
        {
            var product = pair.Value;
            while (product!.HasEvent())
            {
                DomainEvent? @event = product.PopEvent();
                if (@event == null)
                {
                    break;
                }

                yield return Task.FromResult(@event);
            }
        }
    }
    
    public async Task<IEnumerable<DomainEvent>> CollectEvents()
    {
        List<DomainEvent> list = new();
        foreach (KeyValuePair<string, Product> pair in repository.Seen)
        {
            var product = pair.Value;
            while (product!.HasEvent())
            {
                DomainEvent? @event = product.PopEvent();
                if (@event == null)
                {
                    break;
                }

                list.Add(@event);
            }
        }

        return list;
    }
    private async Task DispatchDomainEventsAsync()
    {
        // IAsyncEnumerable<Task<DomainEvent>> collectNewEvents = CollectNewEvents();
        // await foreach (var collectedEvent in collectNewEvents)
        // {
        //     await eventBus.HandleAsync(collectedEvent.Result, this);
        // }

        
        foreach (var @event in await CollectEvents())
        {
            await eventBus.HandleAsync(@event, this);
        }
    }
}