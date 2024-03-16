using DDDOnlineRetailerCSharp.Link;
using DDDOnlineRetailerCSharp.Persistence;

namespace DDDOnlineRetailerCSharp.Test.Fixtures;

public class RepositoryFixture : IDisposable
{
    public RepositoryFixture()
    {
        DbContext = RetailerDbContext.CreateSqliteRetailerDbContext();
        UnitOfWork = new UnitOfWork(DbContext);
        Repository = new Repository(DbContext);
    }

    public RetailerDbContext DbContext { get; }

    public IUnitOfWork UnitOfWork { get; }

    public IRepository Repository { get; }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}