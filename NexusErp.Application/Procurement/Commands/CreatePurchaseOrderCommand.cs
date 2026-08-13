using MediatR;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Commands
{
    public record PurchaseOrderItemDto(Guid ProductId, decimal QuantityOrderd, decimal UnitCost);
    public record CreatePurchaseOrderCommand(
        Guid SupplierId,
        Guid BranchId,
        List<PurchaseOrderItemDto> Items
        ) : IRequest<Result<Guid>>;
}
