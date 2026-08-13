using MediatR;
using Nexus.Erp.Domain.Entities.Catalog;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NexusErp.Application.Procurement.Queries
{
    public record GetProductByBarcodeQuery(string Barcode) : IRequest<Result<ProductDto>>;

    public class GetProductByBarcodeQueryHandler : IRequestHandler<GetProductByBarcodeQuery, Result<ProductDto>>
    {
        private readonly IGenericRepository<Product> _productRepo;

        public GetProductByBarcodeQueryHandler(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<Result<ProductDto>> Handle(GetProductByBarcodeQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepo.GetAllAsync();
            var product = products.FirstOrDefault(p => p.Barcode == request.Barcode);

            if (product == null)
            {
                return Result.Failure<ProductDto>(new Error("NotFound", $"Product with barcode '{request.Barcode}' was not found."));
            }

            var dto = new ProductDto(
                product.Id,
                product.Name,
                product.Barcode,
                product.DefaultUnitCost,
                product.SellingPrice,
                product.CategoryId
            );

            return Result<ProductDto>.Success(dto);
        }
    }
}