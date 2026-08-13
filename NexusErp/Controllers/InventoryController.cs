using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexusErp.Application.Procurement.Queries;
         
namespace NexusErp.API.Controllers;
        
[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{       
    private readonly ISender _mediator;
        
    public InventoryController(ISender mediator)
    {   
        _mediator = mediator;
    }   
        
    [HttpGet("branch/{branchId:guid}/product/{productId:guid}")]
    public async Task<IActionResult> GetProductStock(Guid branchId, Guid productId)
    {   
        var query = new GetProductStockQuery(branchId, productId);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
         
        return Ok(result);
    }   
}       