using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Adaptors;

public interface IRepository
{
    Task AddAsync(Product product);
    Task<Product?> GetAsync(string sku);
}