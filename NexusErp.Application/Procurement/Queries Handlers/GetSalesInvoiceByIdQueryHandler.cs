using MediatR;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using Nexus.Erp.Domain.Entities.Sales;

namespace NexusErp.Application.Sales.Queries;

public record GetSalesInvoiceByIdQuery(Guid Id) : IRequest<Result<SalesInvoiceDto>>;

public class GetSalesInvoiceByIdQueryHandler : IRequestHandler<GetSalesInvoiceByIdQuery, Result<SalesInvoiceDto>>
{
    private readonly IGenericRepository<SalesInvoice> _invoiceRepo;

    public GetSalesInvoiceByIdQueryHandler(IGenericRepository<SalesInvoice> invoiceRepo)
    {
        _invoiceRepo = invoiceRepo;
    }

    public async Task<Result<SalesInvoiceDto>> Handle(GetSalesInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoices = await _invoiceRepo.GetAllAsync();
        var invoice = invoices.FirstOrDefault(i => i.Id == request.Id);

        if (invoice == null)
            return Result.Failure<SalesInvoiceDto>(new Error("NotFound","Sales invoice not found"));

        var dto = new SalesInvoiceDto(
            invoice.Id,
            invoice.BranchId,
            invoice.CreatedByUserId,
            invoice.InvoiceNumber,
            invoice.InvoiceDate,
            invoice.TotalAmount,
            invoice.DiscountAmount,
            invoice.NetAmount,
            invoice.PaymentStatus.ToString(),
            invoice.PaymentMethod.ToString()
        );

        return Result<SalesInvoiceDto>.Success(dto);
    }
}