using MediatR;
using NexusErp.Application.Common.Models;

namespace NexusErp.Application.Sales.Commands
{
    public record SalesReturnItemInputDto(
        Guid ProductId,
        decimal Quantity
    );

    public record CreateSalesReturnCommand(
        Guid SalesInvoiceId,
        Guid CreatedByUserId,
        List<SalesReturnItemInputDto> Items,
        string? Reason
    ) : IRequest<Result<Guid>>;
}