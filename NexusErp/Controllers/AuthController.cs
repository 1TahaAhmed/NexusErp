using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nexus.Erp.Domain.Entities.Identity;
using NexusErp.Application.Identity.Commands;
using NexusErp.Application.Identity.Commands.Login;
using NexusErp.Application.Identity.Commands.Registeration;

namespace NexusErp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
        {
            return BadRequest($"{result.Error}");
        }
        return Ok(result);
    }
}