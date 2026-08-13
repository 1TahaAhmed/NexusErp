using MediatR;
using Nexus.Erp.Domain.Entities.Catalog;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Queries
{
    public record ProductDto(
        Guid Id,
        string Name,
        string Barcode,
        decimal DefaultUnitCost,
        decimal SellingPrice,
        Guid CategoryId
        );

    public record GetProductsListQuery() : IRequest<Result<IReadOnlyList<ProductDto>>>;
    public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, Result<IReadOnlyList<ProductDto>>>
    {
        private readonly IGenericRepository<Product> _productRepo;

        public GetProductsListQueryHandler(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }
        public async Task<Result<IReadOnlyList<ProductDto>>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepo.GetAllAsync();

            IReadOnlyList<ProductDto> dtos = products.Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Barcode,
                p.DefaultUnitCost,
                p.SellingPrice,
                p.CategoryId
                )).ToList();

            return Result<IReadOnlyList<ProductDto>>.Success(dtos);
        }
    }
}
