using MediatR;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using Nexus.Erp.Domain.Entities.Procurement;

namespace NexusErp.Application.Procurement.Queries;

public record GetPurchaseOrderByIdQuery(Guid Id) : IRequest<Result<PurchaseOrderDto>>;

public class GetPurchaseOrderByIdQueryHandler : IRequestHandler<GetPurchaseOrderByIdQuery, Result<PurchaseOrderDto>>
{
    private readonly IGenericRepository<PurchaseOrder> _poRepo;

    public GetPurchaseOrderByIdQueryHandler(IGenericRepository<PurchaseOrder> poRepo)
    {
        _poRepo = poRepo;
    }

    public async Task<Result<PurchaseOrderDto>> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _poRepo.GetAllAsync();
        var order = orders.FirstOrDefault(o => o.Id == request.Id);

        if (order == null)
            return Result.Failure<PurchaseOrderDto>(new Error("NotFound","Purchase order not found"));

        var dto = new PurchaseOrderDto(order.Id, order.SupplierId, order.BranchId, order.Status.ToString());
        return Result<PurchaseOrderDto>.Success(dto);
    }
}