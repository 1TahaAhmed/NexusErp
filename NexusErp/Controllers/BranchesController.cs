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
    public class BranchesController : ControllerBase
    {
        private readonly ISender _mediator;

        public BranchesController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBranch([FromBody] CreateBranchCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }
        [HttpPut("stock-settings")]
        public async Task<IActionResult> UpdateStockSettings([FromBody] UpdateBranchStockSettingsCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetBranches()
        {
            var result = await _mediator.Send(new GetBarnchesListQuery());

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBranchById(Guid id)
        {
            var result = await _mediator.Send(new GetBranchByIdQuery(id));
            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBranch(Guid id, [FromBody] UpdateBranchCommand command)
        {
            if (id != command.Id)
                return BadRequest(new Error("InvalidId", "Route ID does not match body ID"));

            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBranch(Guid id)
        {
            var result = await _mediator.Send(new DeleteBranchCommand(id));
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result);
        }
    }
}
