using MediatR;
using Nexus.Erp.Domain.Entities.Procurement;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NexusErp.Application.Procurement.Queries
{
    public record PurchaseOrderDto(
        Guid Id,
        Guid SupplierId,
        Guid BranchId,
        string Status
    );

    public record GetPurchaseOrdersListQuery() : IRequest<Result<IReadOnlyList<PurchaseOrderDto>>>;

    public class GetPurchaseOrdersListQueryHandler : IRequestHandler<GetPurchaseOrdersListQuery, Result<IReadOnlyList<PurchaseOrderDto>>>
    {
        private readonly IGenericRepository<PurchaseOrder> _poRepo;

        public GetPurchaseOrdersListQueryHandler(IGenericRepository<PurchaseOrder> poRepo)
        {
            _poRepo = poRepo;
        }

        public async Task<Result<IReadOnlyList<PurchaseOrderDto>>> Handle(GetPurchaseOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orders = await _poRepo.GetAllAsync();

            IReadOnlyList<PurchaseOrderDto> dtos = orders.Select(o => new PurchaseOrderDto(
                o.Id,
                o.SupplierId,
                o.BranchId,
                o.Status.ToString()
            )).ToList();

            return Result<IReadOnlyList<PurchaseOrderDto>>.Success(dtos);
        }
    }
}