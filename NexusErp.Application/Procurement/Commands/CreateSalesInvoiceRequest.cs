using NexusErp.Application.Sales.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Commands
{
    public record CreateSalesInvoiceRequest(
        Guid BranchId,
        string? CustomerEmail,
        IReadOnlyList<SalesInvoiceItemInputDto> Items,
        IReadOnlyList<PaymentTransactionInputDto> Payments,
        decimal DiscountAmount = 0
    );
}
