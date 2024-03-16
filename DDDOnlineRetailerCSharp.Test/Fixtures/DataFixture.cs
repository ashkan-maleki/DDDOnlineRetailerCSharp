using DDDOnlineRetailerCSharp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Test.Fixtures;

public class DataFixture : IDisposable
{
    const string Sku = "GENERIC-SOFA";

    public async Task<int> InsertOrderLine(RetailerDbContext dbContext)
    {
        await dbContext.Database.ExecuteSqlAsync($@"
            INSERT INTO order_lines (orderid, sku, qty) VALUES
            ('order1', {Sku}, 12)
            ");
        string orderId = "order1";

        int id = await dbContext.OrderLines.FromSql($"SELECT id FROM order_lines WHERE orderid={orderId} AND sku={Sku}")
            .Select(line => line.Id).FirstAsync();
        return id;
    }

    public async Task<int> InsertBatch(RetailerDbContext dbContext, string reference)
    {
        await dbContext.Database.ExecuteSqlAsync($@"
            INSERT INTO batches (reference, sku, purchasedquantity, eta) VALUES
            ({reference}, {Sku}, 100, null)
            ");

        int id = await dbContext.OrderLines.FromSql($"SELECT id FROM batches WHERE reference={reference} AND sku={Sku}")
            .Select(line => line.Id).FirstAsync();
        return id;
    }

    public async Task InsertAllocation(RetailerDbContext dbContext, int batchId, int orderlineId) =>
        await dbContext.Database.ExecuteSqlAsync($@"
            INSERT INTO BatchOrderLine (batchid, AllocationsId) VALUES
            ({batchId}, {orderlineId})
            ");


    public void Dispose()
    {
        // nothing to dispose.
    }
}