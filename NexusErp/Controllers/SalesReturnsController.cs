namespace NexusErp.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nexus.Erp.Application.SalesReturns.Queries.GetSalesReturnById;
using Nexus.Erp.Domain.Entities.Identity;
using NexusErp.Application.Sales.Commands;
using NexusErp.Application.SalesReturns.Commands.CreateSalesReturn;
using NexusErp.Application.SalesReturns.Queries.GetSalesReturnsList;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class SalesReturnsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesReturnsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReturn([FromBody] CreateSalesReturnCommand command)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(userIdClaim, out var userId))
        {
            command = command with { CreatedByUserId = userId };
        }

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(new { result.Error });
        }

        return Ok(new { id = result.Value });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetSalesReturnByIdQuery(id));

        if (result.IsFailure)
        {
            return NotFound(new { result.Error });
        }

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] GetSalesReturnsListQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return BadRequest(new { result.Error });
        }

        return Ok(result.Value);
    }
}