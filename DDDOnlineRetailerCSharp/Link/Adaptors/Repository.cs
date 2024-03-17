using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Link.Adaptors;

public class Repository(RetailerDbContext dbContext) : IRepository
{
    public async Task AddAsync(Batch batch) => await dbContext.Batches.AddAsync(batch);

    public async Task<Batch> GetAsync(string reference) => await dbContext.Batches.Include(b=> b.Allocations).FirstAsync(b => b.Reference == reference);

    public async Task<List<Batch>> ListAsync() => await dbContext.Batches.Include(b=> b.Allocations).ToListAsync();
}