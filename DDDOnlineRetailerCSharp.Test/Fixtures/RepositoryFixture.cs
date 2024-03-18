using DDDOnlineRetailerCSharp.Link;
using DDDOnlineRetailerCSharp.Link.Adaptors;
using DDDOnlineRetailerCSharp.Link.Services;
using DDDOnlineRetailerCSharp.Persistence;

namespace DDDOnlineRetailerCSharp.Test.Fixtures;

public class RepositoryFixture : IDisposable
{
    public RepositoryFixture()
    {
        DbContext = RetailerDbContext.CreateSqliteRetailerDbContext();
        
        UnitOfWork = new UnitOfWork(DbContext, new Repository(DbContext));
    }

    public RetailerDbContext DbContext { get; }

    public IUnitOfWork UnitOfWork { get; }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}