using MediatR;
using Nexus.Erp.Domain.Entities.Catalog;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NexusErp.Application.Procurement.Commands
{
    public record DeleteProductCommand(Guid Id) : IRequest<Result<bool>>;

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<bool>>
    {
        private readonly IGenericRepository<Product> _productRepo;

        public DeleteProductCommandHandler(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var products = await _productRepo.GetAllAsync();
            var product = products.FirstOrDefault(p => p.Id == request.Id);

            if (product == null)
            {
                return Result.Failure<bool>(new Error("NotFound", "Product not found"));
            }

            _productRepo.DeleteItem(product);

            return Result<bool>.Success(true);
        }
    }
}