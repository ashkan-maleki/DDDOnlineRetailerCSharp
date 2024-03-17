namespace DDDOnlineRetailerCSharp.Link.Services;

public interface IUnitOfWork
{
    void Commit();
    Task CommitAsync();
}