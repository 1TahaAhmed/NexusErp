using MediatR;
using Nexus.Erp.Application.SalesReturns.Queries.GetSalesReturnById;
using Nexus.Erp.Domain.Entities.Sales;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Common.Specifications;

namespace NexusErp.Application.SalesReturns.Queries.GetSalesReturnById
{
    public class GetSalesReturnByIdQueryHandler : IRequestHandler<GetSalesReturnByIdQuery, Result<SalesReturnDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSalesReturnByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SalesReturnDetailsDto>> Handle(GetSalesReturnByIdQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<SalesReturn>();

            var spec = new BaseSpecification<SalesReturn>(x => x.Id == request.Id);
            spec.AddInclude(x => x.SalesReturnItems);

            var salesReturn = await repo.GetEntityWithSpecAsync(spec);

            if (salesReturn is null)
            {
                return Result.Failure<SalesReturnDetailsDto>(
                    new Error("SalesReturn.NotFound", $"Sales Return with ID '{request.Id}' was not found."));
            }

            var dto = new SalesReturnDetailsDto(
                salesReturn.Id,
                salesReturn.SalesInvoiceId,
                salesReturn.BranchId,
                salesReturn.CreatedByUserId,
                salesReturn.ReturnDate,
                salesReturn.TotalRefundAmount,
                salesReturn.Reason,
                salesReturn.SalesReturnItems.Select(item => new SalesReturnItemDto(
                    item.ProductId,
                    (int)item.Quantity,
                    item.UnitPrice,
                    item.Quantity * item.UnitPrice
                )).ToList()
            );

            return Result.Success(dto);
        }
    }
}