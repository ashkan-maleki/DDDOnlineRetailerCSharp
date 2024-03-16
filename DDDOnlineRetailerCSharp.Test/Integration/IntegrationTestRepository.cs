using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link;
using DDDOnlineRetailerCSharp.Persistence;
using DDDOnlineRetailerCSharp.Test.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Test.Integration;

public class IntegrationTestRepository(RepositoryFixture repositoryFixture, DataFixture dataFixture) : IClassFixture<RepositoryFixture>, IClassFixture<DataFixture>
{
    [Fact]
    public async Task TestRepositoryCanSaveABatch()
    {
        Batch batch = new("batch1", "RUSTY-SOAPDISH", 100);

        await repositoryFixture.Repository.AddAsync(batch: batch);
        await repositoryFixture.UnitOfWork.CommitAsync();

        Batch got = await repositoryFixture.DbContext.Batches.FromSql($"SELECT * FROM batches").FirstAsync();
        got.Should().Be(batch);
    }

    [Fact]
    public async Task TestRepositoryCanRetrieveABatchWithAllocations()
    {
        RetailerDbContext dbContext = repositoryFixture.DbContext;
        
        int orderLineID = await dataFixture.InsertOrderLine(dbContext);
        int batch1ID = await dataFixture.InsertBatch(dbContext,"batch1");
        await dataFixture.InsertBatch(dbContext,"batch2");
        await dataFixture.InsertAllocation(dbContext,orderLineID, batch1ID);
        
        Batch got = await repositoryFixture.Repository.GetAsync("batch1");
        Batch expected = new("batch1", "GENERIC-SOFA", 100);
        expected.Should().Be(got);
        expected.Sku.Should().Be(got.Sku);
        expected.PurchasedQuantity.Should().Be(got.PurchasedQuantity);
        List<OrderLine> expectedLine = new() { new("order1", "GENERIC-SOFA", 12) };
        expectedLine.Should().Equal(got.Allocations);
    }
}