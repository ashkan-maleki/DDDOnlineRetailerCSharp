using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Persistence.Seed;

public class FakeFactory
{
    public static void CreateBatch(RetailerDbContext dbContext)
    {
        dbContext.Database.ExecuteSql($@"
             INSERT INTO batches (reference, sku, purchasedquantity, eta)
             VALUES ('batch1', 'sku1', 100, null);
             INSERT INTO batches (reference, sku, purchasedquantity, eta)
             VALUES ('batch2', 'sku2', 200, '2011-04-11');
        ");
    }
}