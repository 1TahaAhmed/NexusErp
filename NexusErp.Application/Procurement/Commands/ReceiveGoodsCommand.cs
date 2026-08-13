using MediatR;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Commands
{
    public record GRNItemInputDto(
        Guid ProductId,
        string BatchNumber,
        DateTime ExpiryDate,
        decimal QuantityReceived,
        decimal QuantityRejected,
        decimal UnitCost
        );
    public record ReceiveGoodsCommand(
        Guid PurchaseOrderId,
        Guid SupplierId,
        Guid BranchId,
        string InvoiceNumber,
        List<GRNItemInputDto> Items
        ) : IRequest<Result<Guid>>;
}
