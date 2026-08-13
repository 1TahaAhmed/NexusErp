using MediatR;
using Nexus.Erp.Domain.Entities.Procurement;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NexusErp.Application.Procurement.Commands
{
    public record UpdateSupplierCommand(Guid Id, string CompanyName, string Phone, string TaxNumber) : IRequest<Result<bool>>;

    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, Result<bool>>
    {
        private readonly IGenericRepository<Supplier> _supplierRepo;

        public UpdateSupplierCommandHandler(IGenericRepository<Supplier> supplierRepo)
        {
            _supplierRepo = supplierRepo;
        }

        public async Task<Result<bool>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var suppliers = await _supplierRepo.GetAllAsync();
            var supplier = suppliers.FirstOrDefault(s => s.Id == request.Id);

            if (supplier == null)
            {
                return Result.Failure<bool>(new Error("NotFound", "Supplier not found"));
            }

            supplier.CompanyName = request.CompanyName;
            supplier.Phone = request.Phone;
            supplier.TaxNumber = request.TaxNumber;

            _supplierRepo.UpdateItem(supplier);

            return Result<bool>.Success(true);
        }
    }
}