using MediatR;
using Nexus.Erp.Domain.Entities.Sales;
using Nexus.Erp.Domain.Enums;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Specifications;
using NexusErp.Application.Payments.Commands;

namespace NexusErp.Application.Payments.Commands;

public record ProcessPaymentSuccessCommand(Guid SaleInvoiceId, long TransactionId) : IRequest<bool>;

public class ProcessPaymentSuccessCommandHandler : IRequestHandler<ProcessPaymentSuccessCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public ProcessPaymentSuccessCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ProcessPaymentSuccessCommand request, CancellationToken cancellationToken)
    {
        var invoiceRepo = _unitOfWork.Repository<SalesInvoice>();

        var spec = new BaseSpecification<SalesInvoice>(x => x.Id == request.SaleInvoiceId);
        var invoice = await invoiceRepo.GetEntityWithSpecAsync(spec);

        if (invoice == null) return false;

        invoice.PaymentStatus = PaymentStatus.Paid;

        invoiceRepo.UpdateItem(invoice);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}