namespace NexusErp.Application.SalesReturns.Commands.CreateSalesReturn;

using MediatR;
using Nexus.Erp.Domain.Entities.Inventory;
using Nexus.Erp.Domain.Entities.Sales;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Common.Specifications;
using NexusErp.Application.Sales.Commands;

public class CreateSalesReturnCommandHandler : IRequestHandler<CreateSalesReturnCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateSalesReturnCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateSalesReturnCommand request, CancellationToken cancellationToken)
    {
        var invoiceRepository = _unitOfWork.Repository<SalesInvoice>();

        var spec = new BaseSpecification<SalesInvoice>(i => i.Id == request.SalesInvoiceId);
        spec.AddInclude(i => i.SalesInvoiceItems);

        var invoice = await invoiceRepository.GetEntityWithSpecAsync(spec);

        if (invoice is null)
        {
            return Result.Failure<Guid>(new Error("SalesInvoice.NotFound", "The specified sales invoice was not found."));
        }

        if (invoice.IsReturned)
        {
            return Result.Failure<Guid>(new Error("SalesInvoice.AlreadyReturned", "A return has already been processed for this invoice."));
        }

        if (request.Items is null || !request.Items.Any())
        {
            return Result.Failure<Guid>(new Error("SalesReturn.EmptyItems", "At least one item must be selected for return."));
        }

        decimal totalRefundAmount = 0;
        var returnItems = new List<SalesReturnItem>();

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var branchStockRepo = _unitOfWork.Repository<BranchStock>();
            var batchRepo = _unitOfWork.Repository<ProductBatch>();

            foreach (var itemInput in request.Items)
            {
                var invoiceItem = invoice.SalesInvoiceItems.FirstOrDefault(x => x.ProductId == itemInput.ProductId);
                if (invoiceItem is null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result.Failure<Guid>(new Error("SalesReturn.InvalidProduct", $"Product {itemInput.ProductId} does not belong to this invoice."));
                }

                if (itemInput.Quantity <= 0 || itemInput.Quantity > invoiceItem.Quantity)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result.Failure<Guid>(new Error("SalesReturn.InvalidQuantity", $"Invalid return quantity for product {itemInput.ProductId}. Purchased: {invoiceItem.Quantity}."));
                }

                var itemRefund = itemInput.Quantity * invoiceItem.UnitPrice;
                totalRefundAmount += itemRefund;

                returnItems.Add(new SalesReturnItem
                {
                    ProductId = itemInput.ProductId,
                    Quantity = itemInput.Quantity,
                    UnitPrice = invoiceItem.UnitPrice
                });

                var stockSpec = new BaseSpecification<BranchStock>(s => s.BranchId == invoice.BranchId && s.ProductId == itemInput.ProductId);
                var branchStock = await branchStockRepo.GetEntityWithSpecAsync(stockSpec);

                if (branchStock is not null)
                {
                    branchStock.QuantityOnHand += itemInput.Quantity;
                    branchStockRepo.UpdateItem(branchStock);
                }

                var batchSpec = new BaseSpecification<ProductBatch>(b => b.ProductId == itemInput.ProductId && b.BranchId == invoice.BranchId);
                var batch = await batchRepo.GetEntityWithSpecAsync(batchSpec);

                if (batch is not null)
                {
                    batch.QuantityAvailable += itemInput.Quantity;
                    batchRepo.UpdateItem(batch);
                }
            }

            var salesReturn = new SalesReturn
            {
                SalesInvoiceId = invoice.Id,
                BranchId = invoice.BranchId,
                CreatedByUserId = request.CreatedByUserId,
                ReturnDate = DateTime.UtcNow,
                TotalRefundAmount = totalRefundAmount,
                Reason = request.Reason,
                SalesReturnItems = returnItems
            };

            invoice.IsReturned = true;
            invoiceRepository.UpdateItem(invoice);

            var salesReturnRepo = _unitOfWork.Repository<SalesReturn>();
            await salesReturnRepo.AddItem(salesReturn);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return Result.Success(salesReturn.Id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            var innerMessage = ex.InnerException?.Message ?? ex.Message;
            return Result.Failure<Guid>(new Error("SalesReturn.ExecutionFailed", innerMessage));
        }
    }
}