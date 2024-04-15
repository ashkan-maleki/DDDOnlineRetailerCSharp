using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Adaptors;
using DDDOnlineRetailerCSharp.Link.Services;
using Microsoft.AspNetCore.Mvc;

namespace DDDOnlineRetailerCSharp.Controllers;

[ApiController]
[Route("api/")]
public class IndexController(IUnitOfWork uow) : ControllerBase
{
    private readonly ProductService _service = new (uow);

    [HttpPost("add_batch")]
    public async Task<IActionResult> AddBatch([FromBody] Batch batch)
    {
        await _service.AddBatch(batch); 
        return StatusCode(201);
    }
    
    [HttpPost("allocate")]
    public async Task<IActionResult> Allocate([FromBody] OrderLine line)
    {
        try
        {
            string batchRef = await _service.Allocate(line);
            return StatusCode(201, batchRef);
        }
        catch (InvalidSku e)
        {
            return BadRequest(e.Message);
        }
        
    }
}