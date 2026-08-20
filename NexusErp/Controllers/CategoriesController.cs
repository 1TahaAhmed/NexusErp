using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Common.Permissions;
using NexusErp.Application.Procurement.Commands;
using NexusErp.Application.Procurement.Queries;

namespace NexusErp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ISender _mediator;

        public CategoriesController(ISender mediator)
        {
            _mediator = mediator;            
        }
        [HttpPost]
        [Authorize(Policy = Permissions.Categories.CreateCategory)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Categories.View)]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _mediator.Send(new GetCategoriesListQuery());

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = Permissions.Categories.View)]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));
            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = Permissions.Categories.EditCategory)]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
                return BadRequest(new Error("InvalidId", "Route ID does not match body ID"));

            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = Permissions.Categories.DeleteCategory)]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id));
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result);
        }
    }
}
