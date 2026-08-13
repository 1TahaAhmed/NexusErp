using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Procurement.Commands;
using NexusErp.Application.Procurement.Queries;

namespace NexusErp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISender _mediator;

        public SuppliersController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers()
        {
            var result = await _mediator.Send(new GetSuppliersListQuery());

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSupplierById(Guid id)
        {
            var result = await _mediator.Send(new GetSupplierByIdQuery(id));
            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSupplier(Guid id, [FromBody] UpdateSupplierCommand command)
        {
            if (id != command.Id)
                return BadRequest(new Error("InvalidId", "Route ID does not match body ID"));

            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSupplier(Guid id)
        {
            var result = await _mediator.Send(new DeleteSupplierCommand(id));
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result);
        }
    }
}
