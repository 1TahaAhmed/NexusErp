using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusErp.Application.Common.Permissions;
using NexusErp.Application.Procurement.Commands;
using NexusErp.Application.Procurement.Queries;

namespace NexusErp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly ISender _mediator;

        public PurchaseOrdersController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = Permissions.PurchaseOrders.CreatePurchaseOrder)]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }
        [HttpPost("receive-goods")]
        [Authorize(Policy = Permissions.PurchaseOrders.ReceiveGoods)]
        public async Task<IActionResult> ReceiveGoods([FromBody] ReceiveGoodsCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }
        [HttpGet]
        [Authorize(Policy = Permissions.PurchaseOrders.View)]
        public async Task<IActionResult> GetPurchaseOrders()
        {
            var result = await _mediator.Send(new GetPurchaseOrdersListQuery());

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }
        [HttpGet("{id:guid}")]
        [Authorize(Policy = Permissions.PurchaseOrders.View)]
        public async Task<IActionResult> GetPurchaseOrderById(Guid id)
        {
            var result = await _mediator.Send(new GetPurchaseOrderByIdQuery(id));
            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result);
        }
    }
}
