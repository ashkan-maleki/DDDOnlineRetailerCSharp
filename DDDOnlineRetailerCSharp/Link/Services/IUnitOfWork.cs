using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Adaptors;

namespace DDDOnlineRetailerCSharp.Link.Services;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository Repository { get; }
    int Commit();
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    IAsyncEnumerable<Task<Event>> CollectNewEvents();
}