using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Test.Integration;

public class IntegrationTestOrm
{
    [Fact]
    public async Task TestOrderLineEntityCanLoadLines()
    {
        RetailerDbContext dbContext = RetailerDbContext.CreateSqliteRetailerDbContext();
        
        await dbContext.Database.ExecuteSqlAsync($@"
            INSERT INTO order_lines (orderid, sku, qty) VALUES
            ('order1', 'RED-CHAIR', 12),
            ('order1', 'RED-TABLE', 13),
            ('order2', 'BLUE-LIPSTICK', 14)
        ");
        
        List<OrderLine> expected =
        [
            new("order1", "RED-CHAIR", 12),
            new("order1", "RED-TABLE", 13),
            new("order2", "BLUE-LIPSTICK", 14)
        ];
        
        dbContext.OrderLines.Count().Should().Be(expected.Count);
        dbContext.OrderLines.ToList().Should().BeEquivalentTo(expected);
    }
    
    
    [Fact]
    public async Task TestOrderLineEntityCanSaveLines()
    {
        RetailerDbContext dbContext = RetailerDbContext.CreateSqliteRetailerDbContext();
        
        List<OrderLine> expected =
        [
            new("order1", "RED-CHAIR", 12),
            new("order1", "RED-TABLE", 13),
            new("order2", "BLUE-LIPSTICK", 14)
        ];
        
        await dbContext.OrderLines.AddRangeAsync(expected);
        await dbContext.SaveChangesAsync();
        List<OrderLine> actual = dbContext.OrderLines.FromSql($"SELECT id, orderid, sku, qty FROM order_lines").ToList();

        expected.Count.Should().Be(actual.Count);
        expected.Should().BeEquivalentTo(actual);
    }

    [Fact]
    public async Task TestRetrievingBatches()
    {
        RetailerDbContext dbContext = RetailerDbContext.CreateSqliteRetailerDbContext();
        
        await dbContext.Database.ExecuteSqlAsync($@"
             INSERT INTO batches (reference, sku, purchasedquantity, eta)
             VALUES ('batch1', 'sku1', 100, null);
             INSERT INTO batches (reference, sku, purchasedquantity, eta)
             VALUES ('batch2', 'sku2', 200, '2011-04-11');
        ");
        
        List<Batch> expected =
        [
            new("batch1", "sku1", 100),
            new("batch2", "sku2", 200, eta:new DateTime(2011, 4, 11))
        ];
        
        dbContext.Batches.Count().Should().Be(expected.Count);
        dbContext.Batches.ToList().Should().BeEquivalentTo(expected);
    }
}