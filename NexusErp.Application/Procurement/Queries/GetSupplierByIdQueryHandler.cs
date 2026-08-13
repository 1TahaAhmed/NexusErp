using MediatR;
using Nexus.Erp.Domain.Entities.Procurement;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Queries
{
    public record GetSupplierByIdQuery(Guid Id) : IRequest<Result<SupplierDto>>;

    public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, Result<SupplierDto>>
    {
        private readonly IGenericRepository<Supplier> _supplierRepo;

        public GetSupplierByIdQueryHandler(IGenericRepository<Supplier> supplierRepo)
        {
            _supplierRepo = supplierRepo;
        }
        public async Task<Result<SupplierDto>> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await _supplierRepo.GetAllAsync();
            var supplier = suppliers.FirstOrDefault(s => s.Id == request.Id);

            if (supplier == null)
            {
                return Result.Failure<SupplierDto>(new Error("NotFound", "Supplier Not found"));
            }

            var dto = new SupplierDto(supplier.Id, supplier.CompanyName, supplier.Phone, supplier.TaxNumber);
            return Result<SupplierDto>.Success(dto);
        }
    }
}
