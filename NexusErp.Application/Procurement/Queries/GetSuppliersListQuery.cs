using MediatR;
using Nexus.Erp.Domain.Entities.Procurement;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;

namespace NexusErp.Application.Procurement.Queries;

public record SupplierDto(Guid Id, string CompanyName, string Phone, string TaxNumber);

public record GetSuppliersListQuery() : IRequest<Result<IReadOnlyList<SupplierDto>>>;

public class GetSuppliersListQueryHandler : IRequestHandler<GetSuppliersListQuery, Result<IReadOnlyList<SupplierDto>>>
{
    private readonly IGenericRepository<Supplier> _supplierRepo;

    public GetSuppliersListQueryHandler(IGenericRepository<Supplier> supplierRepo)
    {
        _supplierRepo = supplierRepo;
    }

    public async Task<Result<IReadOnlyList<SupplierDto>>> Handle(GetSuppliersListQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await _supplierRepo.GetAllAsync();

        IReadOnlyList<SupplierDto> dtos = suppliers.Select(s => new SupplierDto(
            s.Id,
            s.CompanyName,
            s.Phone,
            s.TaxNumber
        )).ToList();

        return Result<IReadOnlyList<SupplierDto>>.Success(dtos);
    }
}