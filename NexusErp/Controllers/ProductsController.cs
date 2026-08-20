using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Common.Permissions;
using NexusErp.Application.Procurement.Commands;
using NexusErp.Application.Procurement.Queries;
using NexusErp.Application.Procurement.Specifications;

namespace NexusErp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ISender _mediator;

    public ProductsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Products.AddProduct)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpGet("by-barcode/{barcode}")]
    [Authorize(Policy = Permissions.Products.View)]
    public async Task<IActionResult> GetByBarcode(string barcode)
    {
        var result = await _mediator.Send(new GetProductByBarcodeQuery(barcode));

        if (!result.IsSuccess)
        {
            return NotFound(result.Error);
        }

        return Ok(result);
    }
    [HttpGet]
    [Authorize(Policy = Permissions.Products.View)]
    public async Task<IActionResult> GetProducts()
    {
        var result = await _mediator.Send(new GetProductsListQuery());

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = Permissions.Products.EditProduct)]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest(new Error("InvalidId", "Route ID does not match body ID"));

        var result = await _mediator.Send(command);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Permissions.Products.DeleteProduct)]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result);
    }
}