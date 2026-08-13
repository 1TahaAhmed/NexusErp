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
    public record UpdateProductCommand(
        Guid Id,
        string Name,
        string Barcode,
        decimal DefaultUnitCost,
        decimal SellingPrice,
        Guid CategoryId
    ) : IRequest<Result<bool>>;

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<bool>>
    {
        private readonly IGenericRepository<Product> _productRepo;

        public UpdateProductCommandHandler(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<Result<bool>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var products = await _productRepo.GetAllAsync();
            var product = products.FirstOrDefault(p => p.Id == request.Id);

            if (product == null)
            {
                return Result.Failure<bool>(new Error("NotFound", "Product not found"));
            }

            product.Name = request.Name;
            product.Barcode = request.Barcode;
            product.DefaultUnitCost = request.DefaultUnitCost;
            product.SellingPrice = request.SellingPrice;
            product.CategoryId = request.CategoryId;

            _productRepo.UpdateItem(product);

            return Result<bool>.Success(true);
        }
    }
}