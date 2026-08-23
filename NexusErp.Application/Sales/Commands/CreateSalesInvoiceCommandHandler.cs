using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexus.Erp.Domain.Entities.Catalog;
using Nexus.Erp.Domain.Entities.Inventory;
using Nexus.Erp.Domain.Entities.Sales;
using Nexus.Erp.Domain.Enums;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Common.Specifications;
using NexusErp.Application.Inventory;
using NexusErp.Application.Procurement.Specifications;

namespace NexusErp.Application.Sales.Commands;

public class CreateSalesInvoiceCommandHandler : IRequestHandler<CreateSalesInvoiceCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGatewayService _paymentGatewayService;

    public CreateSalesInvoiceCommandHandler(IUnitOfWork unitOfWork, IPaymentGatewayService paymentGatewayService)
    {
        _unitOfWork = unitOfWork;
        _paymentGatewayService = paymentGatewayService;
    }

    public async Task<Result<Guid>> Handle(CreateSalesInvoiceCommand request, CancellationToken cancellationToken)
    {
        if (request.Items == null || !request.Items.Any())
        {
            return Result.Failure<Guid>(new Error("SalesInvoice.EmptyItems", "Invoice cannot be created without items."));
        }

        if (request.Payments == null || !request.Payments.Any())
        {
            return Result.Failure<Guid>(new Error("SalesInvoice.EmptyPayments", "At least one payment method must be provided."));
        }

        var batchRepo = _unitOfWork.Repository<ProductBatch>();
        var stockRepo = _unitOfWork.Repository<BranchStock>();
        var invoiceRepo = _unitOfWork.Repository<SalesInvoice>();
        var productRepo = _unitOfWork.Repository<Product>();

        var invoice = new SalesInvoice
        {
            BranchId = request.BranchId,
            CreatedByUserId = request.CreateByUserId,
            InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}",
            InvoiceDate = DateTime.UtcNow,
            DiscountAmount = request.DiscountAmount,
            SalesInvoiceItems = new List<SalesInvoiceItem>(),
            PaymentTransactions = new List<PaymentTransaction>()
        };

        decimal calculatedTotalAmount = 0;

        foreach (var item in request.Items)
        {
            var productSpec = new BaseSpecification<Product>(p => p.Id == item.ProductId);
            var product = await productRepo.GetEntityWithSpecAsync(productSpec);
            if (product == null)
            {
                return Result.Failure<Guid>(new Error("SalesInvoice.ProductNotFound", $"Product with ID {item.ProductId} was not found."));
            }

            var actualUnitPrice = product.SellingPrice;

            var stockSpec = new BranchStockByBranchAndProductSpecification(request.BranchId, item.ProductId);
            var branchStock = await stockRepo.GetEntityWithSpecAsync(stockSpec);

            if (branchStock == null || branchStock.QuantityAvailable < item.Quantity)
            {
                return Result.Failure<Guid>(new Error("SalesInvoice.InsufficientStock", "The available stock is not enough for the selected product."));
            }

            var fifoSpec = new AvailableBatchesFifoSpecification(request.BranchId, item.ProductId);
            var availableBatches = await batchRepo.GetAllWithSpecAsync(fifoSpec);

            decimal remainingQtyToDeduct = item.Quantity;

            foreach (var batch in availableBatches)
            {
                if (remainingQtyToDeduct <= 0) break;

                decimal qtyDeductedFromBatch = Math.Min(batch.QuantityAvailable, remainingQtyToDeduct);

                batch.QuantityAvailable -= qtyDeductedFromBatch;
                batchRepo.UpdateItem(batch);

                var invoiceItem = new SalesInvoiceItem
                {
                    ProductId = item.ProductId,
                    ProductBatchId = batch.Id,
                    Quantity = qtyDeductedFromBatch,
                    UnitPrice = actualUnitPrice,
                };

                invoice.SalesInvoiceItems.Add(invoiceItem);
                calculatedTotalAmount += qtyDeductedFromBatch * actualUnitPrice;

                remainingQtyToDeduct -= qtyDeductedFromBatch;
            }

            if (remainingQtyToDeduct > 0)
            {
                return Result.Failure<Guid>(new Error("SalesInvoice.BatchDeficit", "Unable to allocate the requested quantity from available batches."));
            }

            branchStock.QuantityOnHand -= item.Quantity;
            stockRepo.UpdateItem(branchStock);
        }

        invoice.TotalAmount = calculatedTotalAmount;

        decimal totalPaidAmount = request.Payments.Sum(p => p.Amount);

        if (totalPaidAmount >= invoice.NetAmount)
        {
            invoice.PaymentStatus = PaymentStatus.Paid;
        }
        else if (totalPaidAmount > 0)
        {
            invoice.PaymentStatus = PaymentStatus.PartiallyPaid;
        }
        else
        {
            invoice.PaymentStatus = PaymentStatus.Pending;
        }

        foreach (var paymentInput in request.Payments)
        {
            invoice.PaymentTransactions.Add(new PaymentTransaction
            {
                PaymentMethod = paymentInput.PaymentMethod,
                Amount = paymentInput.Amount,
                GatewayProvider = paymentInput.GatewayProvider,
                TransactionReference = paymentInput.TransactionReference,
                IsSuccess = true,
                TransactionDate = DateTime.UtcNow
            });
        }

        try
        {
            await invoiceRepo.AddItem(invoice);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Failure<Guid>(new Error("SalesInvoice.ConcurrencyConflict",
                "The stock was modified by another cashier at the same time. Please try again."));
        }

        return Result.Success(invoice.Id);
    }
}