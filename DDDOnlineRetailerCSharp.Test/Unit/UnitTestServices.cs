using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Services;
using DDDOnlineRetailerCSharp.Test.Fixtures;
using FluentAssertions;

namespace DDDOnlineRetailerCSharp.Test.Unit;

public class UnitTestServices(RepositoryFixture repositoryFixture) : IClassFixture<RepositoryFixture>
{
    [Fact]
    public async Task TestAddBatch()
    {
        string? reference = "b1";
        Batch batch = new(reference, "CRUNCHY-ARMCHAIR", 100);
        BatchService service = new BatchService(repositoryFixture.UnitOfWork);
        await service.AddBatch(batch);
        Batch got = await repositoryFixture.UnitOfWork.Repository.GetAsync(reference);
        got.Reference.Should().Be(reference);
    }
    
    [Fact]
    public async Task TestReturnsAllocation()
    {
        OrderLine line = new("order1", "COMPLICATED-LAMP", 12);
        Batch batch = new("b1", "COMPLICATED-LAMP", 100);
        await repositoryFixture.UnitOfWork.Repository.AddAsync(batch);
        await repositoryFixture.UnitOfWork.CommitAsync();
        BatchService service = new( repositoryFixture.UnitOfWork);
        string result = await service.Allocate(line);
        result.Should().Be("b1");
    }

    [Fact]
    public async Task TestErrorForInvalidSku()
    {
        OrderLine line = new("o1", "NONEXISTENTSKU", 10);
        Batch batch = new("b1", "AREALSKU", 100);
        await repositoryFixture.UnitOfWork.Repository.AddAsync(batch);
        await repositoryFixture.UnitOfWork.CommitAsync();
        BatchService service = new(repositoryFixture.UnitOfWork);

        async Task Allocate() => await service.Allocate(line);
        InvalidSku exception = await Assert.ThrowsAsync<InvalidSku>(Allocate);
        exception.Message.Should().Be("Invalid sku NONEXISTENTSKU");
    }
}