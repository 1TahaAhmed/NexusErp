using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexusErp.Application.Payments.Commands;

namespace NexusErp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ISender _mediator;

    public PaymentsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("process-success")]
    public async Task<IActionResult> ProcessPaymentSuccess([FromBody] ProcessPaymentSuccessCommand command)
    {
        bool isSuccess = await _mediator.Send(command);

        if (!isSuccess)
        {
            return BadRequest("Payment processing failed.");
        }

        return Ok(isSuccess);
    }
}