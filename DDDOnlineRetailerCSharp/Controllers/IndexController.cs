using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Adaptors;
using DDDOnlineRetailerCSharp.Link.Services;
using Microsoft.AspNetCore.Mvc;

namespace DDDOnlineRetailerCSharp.Controllers;

[ApiController]
[Route("api/")]
public class IndexController(IUnitOfWork uow, IRepository repository) : ControllerBase
{
    [HttpPost("allocate")]
    public async Task<IActionResult> Allocate([FromBody] OrderLine line)
    {
        try
        {
            BatchService service = new BatchService(repository, uow);
            string batchRef = await service.Allocate(line);
            return StatusCode(201, batchRef);
        }
        catch (OutOfStock e)
        {
            return BadRequest(e.Message);
        }
        
    }
}