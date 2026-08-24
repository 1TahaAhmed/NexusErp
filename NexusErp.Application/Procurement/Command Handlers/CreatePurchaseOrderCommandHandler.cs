using MediatR;
using Nexus.Erp.Domain.Entities.Procurement;
using Nexus.Erp.Domain.Enums;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Procurement.Commands;

namespace NexusErp.Application.Procurement.Handlers
{
    public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePurchaseOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.Items == null || !request.Items.Any())
            {
                return Result.Failure<Guid>(new Error("PurchaseOrder.EmptyItems", "You need to add at least one product to the purchase order."));
            }

            var po = new PurchaseOrder
            {
                SupplierId = request.SupplierId,
                BranchId = request.BranchId,
                OrderDate = DateTime.UtcNow,
                Status = PurchaseOrderStatus.Pending,
                Items = request.Items.Select(item => new PurchaseOrderItem
                {
                    ProductId = item.ProductId,
                    QuantityOrdered = item.QuantityOrderd,
                    UnitCost = item.UnitCost,
                }).ToList()
            };

            var poRepo = _unitOfWork.Repository<PurchaseOrder>();
            await poRepo.AddItem(po);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(po.Id);
        }
    }
}
