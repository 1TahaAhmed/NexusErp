using MediatR;
using NexusErp.Application.Common.Models;

namespace NexusErp.Application.SalesReturns.Queries.GetSalesReturnsList
{
    public record SalesReturnListDto(
        Guid Id,
        Guid SalesInvoiceId,
        Guid BranchId,
        DateTime ReturnDate,
        decimal TotalRefundAmount,
        string? Reason,
        int TotalItemsCount);

    public record GetSalesReturnsListQuery(
        int PageNumber = 1,
        int PageSize = 10,
        Guid? BranchId = null,
        DateTime? FromDate = null,
        DateTime? ToDate = null
    ) : IRequest<Result<List<SalesReturnListDto>>>;
}