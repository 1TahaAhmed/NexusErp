using MediatR;
using Nexus.Erp.Domain.Entities.Inventory;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Procurement.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Commands
{
    public class UpdateBranchStockSettingsCommandHandler : IRequestHandler<UpdateBranchStockSettingsCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBranchStockSettingsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(UpdateBranchStockSettingsCommand request, CancellationToken cancellationToken)
        {
            var stockRepo = _unitOfWork.Repository<BranchStock>();

            var spec = new BranchStockByBranchAndProductSpecification(request.BranchId, request.ProductId);
            var branchStock = await stockRepo.GetEntityWithSpecAsync(spec);

            if (branchStock == null) 
            {
                branchStock = new BranchStock
                {
                    BranchId = request.BranchId,
                    ProductId = request.ProductId,
                    ReorderLevel = request.ReorderLevel,
                    QuantityOnHand = 0,
                    QuantityReserved = 0
                };

                await stockRepo.AddItem(branchStock);
            }
            else
            {
                branchStock.ReorderLevel = request.ReorderLevel;
                stockRepo.UpdateItem(branchStock);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Success(branchStock.Id);
        }
    }
}
