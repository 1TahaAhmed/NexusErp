using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusErp.Application.Common.Permissions;
using NexusErp.Application.Procurement.Commands;
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
        public async Task<IActionResult> CreateSalesInvoices([FromBody] CreateSalesInvoiceRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var command = new CreateSalesInvoiceCommand(
                request.BranchId,
                userId,
                request.CustomerEmail,
                request.Items,
                request.Payments,
                request.DiscountAmount
            );

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
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