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
    public record DeleteSupplierCommand(Guid Id) : IRequest<Result<bool>>;

    public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, Result<bool>>
    {
        private readonly IGenericRepository<Supplier> _supplierRepo;

        public DeleteSupplierCommandHandler(IGenericRepository<Supplier> supplierRepo)
        {
            _supplierRepo = supplierRepo;
        }

        public async Task<Result<bool>> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var suppliers = await _supplierRepo.GetAllAsync();
            var supplier = suppliers.FirstOrDefault(s => s.Id == request.Id);

            if (supplier == null)
            {
                return Result.Failure<bool>(new Error("NotFound", "Supplier not found"));
            }

            _supplierRepo.DeleteItem(supplier);

            return Result<bool>.Success(true);
        }
    }
}