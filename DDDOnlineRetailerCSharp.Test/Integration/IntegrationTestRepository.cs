using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link;

namespace DDDOnlineRetailerCSharp.Test.Integration;

public class IntegrationTestRepository
{

    [Fact]
    public async Task TestRepositoryCanSaveABatch()
    {
        IRepository repository;
        IUnitOfWork unitOfWork;
        
        Batch batch = new("batch1", "RUSTY-SOAPDISH", 100);
        
        await repository.AddAsync(batch: batch);
        await unitOfWork.CommitAsync();
        
        
    }
}