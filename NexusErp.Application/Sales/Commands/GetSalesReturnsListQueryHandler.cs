using MediatR;
using Nexus.Erp.Domain.Entities.Sales;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Common.Specifications;

namespace NexusErp.Application.SalesReturns.Queries.GetSalesReturnsList
{
    public class GetSalesReturnsListQueryHandler : IRequestHandler<GetSalesReturnsListQuery, Result<List<SalesReturnListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSalesReturnsListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<SalesReturnListDto>>> Handle(GetSalesReturnsListQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<SalesReturn>();

            var spec = new BaseSpecification<SalesReturn>(x =>
                (!request.BranchId.HasValue || x.BranchId == request.BranchId.Value) &&
                (!request.FromDate.HasValue || x.ReturnDate >= request.FromDate.Value) &&
                (!request.ToDate.HasValue || x.ReturnDate <= request.ToDate.Value)
            );

            spec.AddInclude(x => x.SalesReturnItems);

            var items = await repo.GetAllAsync();

            var dtos = items
                .OrderByDescending(x => x.ReturnDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new SalesReturnListDto(
                    x.Id,
                    x.SalesInvoiceId,
                    x.BranchId,
                    x.ReturnDate,
                    x.TotalRefundAmount,
                    x.Reason,
                    x.SalesReturnItems?.Count ?? 0
                )).ToList();

            return Result.Success(dtos);
        }
    }
}