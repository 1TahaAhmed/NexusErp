using MediatR;
using Nexus.Erp.Domain.Entities.Procurement;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Procurement.Commands;
using NexusErp.Application.Procurement.Specifications;

namespace Nexus.Erp.Application.Procurement.Commands.CreateSupplier;

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateSupplierCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplierRepo = _unitOfWork.Repository<Supplier>();

        var spec = new SupplierByTaxNumberSpecification(request.TaxNumber);
        var existingSupplier = await supplierRepo.GetEntityWithSpecAsync(spec);

        if (existingSupplier != null)
        {
            return Result.Failure<Guid>(new Error("Supplier.DuplicateTaxNumber", "There is already a supplier registered with this tax number."));
        }

        var supplier = new Supplier
        {
            CompanyName = request.CompanyName,
            Phone = request.Phone,
            TaxNumber = request.TaxNumber
        };

        await supplierRepo.AddItem(supplier);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success(supplier.Id);
    }
}