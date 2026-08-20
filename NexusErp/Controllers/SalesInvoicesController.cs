using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusErp.Application.Common.Permissions;
using NexusErp.Application.Payments.Commands;
using NexusErp.Application.Sales.Commands;
using NexusErp.Application.Sales.Queries;

namespace NexusErp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesInvoicesController : ControllerBase
    {
        private readonly ISender _mediator;

        public SalesInvoicesController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Sales.CreateInvoice)]
        public async Task<IActionResult> CreateSalesInvoices([FromBody] CreateSalesInvoiceCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        [HttpPost("process-payment-success")]
        public async Task<IActionResult> ProcessPaymentSuccess([FromBody] ProcessPaymentSuccessCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest(new { Error = "PaymentProcessingFailed", Message = "Failed to process payment." });
            }

            return Ok(new { Message = "Payment updated successfully." });
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Sales.View)]
        public async Task<IActionResult> GetSalesInvoices()
        {
            var result = await _mediator.Send(new GetSalesInvoicesListQuery());

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = Permissions.Sales.View)]
        public async Task<IActionResult> GetSalesInvoiceById(Guid id)
        {
            var result = await _mediator.Send(new GetSalesInvoiceByIdQuery(id));
            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result);
        }
    }
}
