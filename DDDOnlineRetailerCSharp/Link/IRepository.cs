using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link;

public interface IRepository
{
    Task AddAsync(Batch batch);
    Task<Batch> GetAsync(string reference);
    Task<List<Batch>> ListAsync();
}