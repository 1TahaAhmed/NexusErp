using MediatR;
using Nexus.Erp.Domain.Enums;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Sales.Commands
{
    public record SalesInvoiceItemInputDto(
        Guid ProductId,
        decimal Quantity,
        decimal UnitPrice
        );

    public record PaymentTransactionInputDto(
        PaymentMethod PaymentMethod,
        decimal Amount,
        string GatewayProvider="",
        string TransactionReference=""
        );

    public record CreateSalesInvoiceCommand(
        Guid BranchId,
        Guid CreateByUserId,
        string? CustomerEmail,
        List<SalesInvoiceItemInputDto> Items,
        List<PaymentTransactionInputDto> Payments,
        decimal DiscountAmount = 0
        ) : IRequest<Result<Guid>>;
}
