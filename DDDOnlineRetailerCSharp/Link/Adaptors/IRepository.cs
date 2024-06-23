using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Adaptors;

public interface IRepository
{
    Dictionary<string, Product> Seen { get;  }
    Task AddAsync(Product product);
    Task<Product?> GetAsync(string sku);
    Task<Product?> GetByBatchRef(string reference);
}