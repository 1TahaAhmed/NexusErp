using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexusErp.Application.Identity.Commands;
using NexusErp.Application.Identity.Commands.Login;
using NexusErp.Application.Identity.Commands.Registeration; // عدل الـ namespace حسب مكان الـ Auth Commands عندك

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
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}