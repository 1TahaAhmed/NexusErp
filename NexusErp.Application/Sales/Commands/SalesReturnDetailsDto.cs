using MediatR;
using Nexus.Erp.Domain.Common;
using NexusErp.Application.Common.Models;

namespace Nexus.Erp.Application.SalesReturns.Queries.GetSalesReturnById;

public record SalesReturnDetailsDto(
    Guid Id,
    Guid SalesInvoiceId,
    Guid BranchId,
    Guid CreatedByUserId,
    DateTime ReturnDate,
    decimal TotalRefundAmount,
    string? Reason,
    List<SalesReturnItemDto> Items);

public record SalesReturnItemDto(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);

public record GetSalesReturnByIdQuery(Guid Id) : IRequest<Result<SalesReturnDetailsDto>>;