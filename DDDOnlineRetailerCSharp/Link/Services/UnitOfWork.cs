using DDDOnlineRetailerCSharp.Link.Adaptors;
using DDDOnlineRetailerCSharp.Persistence;

namespace DDDOnlineRetailerCSharp.Link.Services;

public class UnitOfWork(RetailerDbContext dbContext, IRepository repository) : IUnitOfWork
{
    public IRepository Repository { get; } = repository;
    public int Commit() => dbContext.SaveChanges();

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default) => await dbContext.SaveChangesAsync(cancellationToken);
    public async ValueTask DisposeAsync() => await dbContext.DisposeAsync();
}