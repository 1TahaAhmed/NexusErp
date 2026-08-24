using MediatR;
using Nexus.Erp.Domain.Entities.Inventory;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Common.Specifications;

namespace NexusErp.Application.Procurement.Queries
{
    public class GetProductStockQueryHandler : IRequestHandler<GetProductStockQuery, Result<decimal>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductStockQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<decimal>> Handle(GetProductStockQuery request, CancellationToken cancellationToken)
        {
            var stockRepo = _unitOfWork.Repository<BranchStock>();

            var spec = new BaseSpecification<BranchStock>(s =>
                s.BranchId == request.BranchId && s.ProductId == request.ProductId);

            var stock = await stockRepo.GetEntityWithSpecAsync(spec);

            if (stock is null)
            {
                return Result.Success(0m);
            }

            return Result.Success(stock.QuantityOnHand);
        }
    }
}