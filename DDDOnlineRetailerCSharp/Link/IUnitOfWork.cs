namespace DDDOnlineRetailerCSharp.Link;

public interface IUnitOfWork
{
    void Commit();
    Task CommitAsync();
}