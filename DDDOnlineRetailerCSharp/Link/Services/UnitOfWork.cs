using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Adaptors;
using DDDOnlineRetailerCSharp.Persistence;

namespace DDDOnlineRetailerCSharp.Link.Services;

public class UnitOfWork(RetailerDbContext dbContext, IRepository repository, IMessageBus messageBus) : IUnitOfWork
{
    public IRepository Repository { get; } = repository;
    public int Commit() => dbContext.SaveChanges();

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        int commit = await dbContext.SaveChangesAsync(cancellationToken);
        return commit;
    }

    public async ValueTask DisposeAsync() => await dbContext.DisposeAsync();

    public async IAsyncEnumerable<Task<Event>> CollectNewEvents()
    {
        foreach (Product product in repository.Seen)
        {
            while (product!.HasEvent())
            {
                Event? @event = product.PopEvent();
                if (@event == null)
                {
                    break;
                }

                yield return Task.FromResult(@event);
            }
        }
    }
}