using DDDOnlineRetailerCSharp.Persistence;

namespace DDDOnlineRetailerCSharp.Link;

public class UnitOfWork(RetailerDbContext dbContext) : IUnitOfWork
{
    public void Commit() => dbContext.SaveChanges();
    public async Task CommitAsync() => await dbContext.SaveChangesAsync();
}