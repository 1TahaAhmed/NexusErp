using MediatR;
using Nexus.Erp.Domain.Entities.Catalog;
using Nexus.Erp.Domain.Entities.Inventory;
using Nexus.Erp.Domain.Entities.Procurement;
using Nexus.Erp.Domain.Enums;
using NexusErp.Application.Catalog.Specifications;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Procurement.Specifications;

namespace NexusErp.Application.Procurement.Commands;

public class ReceiveGoodsCommandHandler : IRequestHandler<ReceiveGoodsCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ReceiveGoodsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(ReceiveGoodsCommand request, CancellationToken cancellationToken)
    {
        var poRepo = _unitOfWork.Repository<PurchaseOrder>();
        var poSpec = new PurchaseOrderByIdSpecification(request.PurchaseOrderId);
        var purchaseOrder = await poRepo.GetEntityWithSpecAsync(poSpec);

        if (purchaseOrder == null)
        {
            return Result.Failure<Guid>(new Error("PurchaseOrder.NotFound", "Purchase Order Not Found"));
        }

        var grn = new GoodsReceiptNote
        {
            PurchaseOrderId = request.PurchaseOrderId,
            SupplierId = request.SupplierId,
            BranchId = request.BranchId,
            InvoiceNumber = request.InvoiceNumber,
            ReceivedDate = DateTime.UtcNow
        };

        var grnRepo = _unitOfWork.Repository<GoodsReceiptNote>();
        var batchRepo = _unitOfWork.Repository<ProductBatch>();
        var stockRepo = _unitOfWork.Repository<BranchStock>();
        var productRepo = _unitOfWork.Repository<Product>();

        foreach (var itemInput in request.Items)
        {
            var grnItem = new GRNItem
            {
                ProductId = itemInput.ProductId,
                BatchNumber = itemInput.BatchNumber,
                ExpiryDate = itemInput.ExpiryDate,
                QuantityReceived = itemInput.QuantityReceived,
                QuantityRejected = itemInput.QuantityRejected,
                UnitCost = itemInput.UnitCost,
            };

            grn.GRNItems.Add(grnItem);

            decimal acceptedQty = grnItem.QuantityAccepted;

            if (acceptedQty > 0)
            {
                var batch = new ProductBatch
                {
                    BranchId = request.BranchId,
                    ProductId = itemInput.ProductId,
                    GRNItem = grnItem,
                    BatchNumber = itemInput.BatchNumber,
                    ExpiryDate = itemInput.ExpiryDate,
                    InitialQuantity = acceptedQty,
                    QuantityAvailable = acceptedQty,
                    UnitCost = itemInput.UnitCost
                };
                await batchRepo.AddItem(batch);

                var stockSpec = new BranchStockByBranchAndProductSpecification(request.BranchId, itemInput.ProductId);
                var branchStock = await stockRepo.GetEntityWithSpecAsync(stockSpec);

                if (branchStock == null)
                {
                    branchStock = new BranchStock
                    {
                        BranchId = request.BranchId,
                        ProductId = itemInput.ProductId,
                        QuantityOnHand = acceptedQty,
                        QuantityReserved = 0,
                        ReorderLevel = 0
                    };
                    await stockRepo.AddItem(branchStock);
                }
                else
                {
                    branchStock.QuantityOnHand += acceptedQty;
                    stockRepo.UpdateItem(branchStock);
                }

                var productSpec = new ProductByIdSpecification(itemInput.ProductId);
                var product = await productRepo.GetEntityWithSpecAsync(productSpec);

                if (product != null)
                {
                    decimal previousQty = branchStock.QuantityOnHand - acceptedQty;
                    decimal previousTotalCost = previousQty * product.DefaultUnitCost;
                    decimal newIncomingCost = acceptedQty * itemInput.UnitCost;
                    decimal newTotalQty = previousQty + acceptedQty;

                    if (newTotalQty > 0)
                    {
                        product.DefaultUnitCost = (previousTotalCost + newIncomingCost) / newTotalQty;
                        productRepo.UpdateItem(product);
                    }
                }
            }
        }

        purchaseOrder.Status = PurchaseOrderStatus.PartiallyReceived;
        poRepo.UpdateItem(purchaseOrder);

        await grnRepo.AddItem(grn);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(grn.Id);
    }
}