using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Adaptors;

public interface IRepository
{
    ICollection<Product> Seen { get;  }
    Task AddAsync(Product product);
    Task<Product?> GetAsync(string sku);
}