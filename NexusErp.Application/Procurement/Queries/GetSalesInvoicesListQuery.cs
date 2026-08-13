using MediatR;
using Nexus.Erp.Domain.Entities.Sales;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NexusErp.Application.Sales.Queries
{
    public record SalesInvoiceDto(
        Guid Id,
        Guid BranchId,
        Guid CreatedByUserId,
        string InvoiceNumber,
        DateTime InvoiceDate,
        decimal TotalAmount,
        decimal DiscountAmount,
        decimal NetAmount,
        string PaymentStatus,
        string PaymentMethod
    );

    public record GetSalesInvoicesListQuery() : IRequest<Result<IReadOnlyList<SalesInvoiceDto>>>;

    public class GetSalesInvoicesListQueryHandler : IRequestHandler<GetSalesInvoicesListQuery, Result<IReadOnlyList<SalesInvoiceDto>>>
    {
        private readonly IGenericRepository<SalesInvoice> _invoiceRepo;

        public GetSalesInvoicesListQueryHandler(IGenericRepository<SalesInvoice> invoiceRepo)
        {
            _invoiceRepo = invoiceRepo;
        }

        public async Task<Result<IReadOnlyList<SalesInvoiceDto>>> Handle(GetSalesInvoicesListQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _invoiceRepo.GetAllAsync();

            IReadOnlyList<SalesInvoiceDto> dtos = invoices.Select(i => new SalesInvoiceDto(
                i.Id,
                i.BranchId,
                i.CreatedByUserId,
                i.InvoiceNumber,
                i.InvoiceDate,
                i.TotalAmount,
                i.DiscountAmount,
                i.NetAmount,
                i.PaymentStatus.ToString(),
                i.PaymentMethod.ToString()
            )).ToList();

            return Result<IReadOnlyList<SalesInvoiceDto>>.Success(dtos);
        }
    }
}