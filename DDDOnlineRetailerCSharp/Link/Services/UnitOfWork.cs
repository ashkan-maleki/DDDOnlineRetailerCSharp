using DDDOnlineRetailerCSharp.Persistence;

namespace DDDOnlineRetailerCSharp.Link.Services;

public class UnitOfWork(RetailerDbContext dbContext) : IUnitOfWork
{
    public void Commit() => dbContext.SaveChanges();

    public async Task CommitAsync() => await dbContext.SaveChangesAsync();
}