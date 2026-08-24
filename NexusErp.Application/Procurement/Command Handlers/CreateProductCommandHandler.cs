using MediatR;
using Nexus.Erp.Application.Catalog.Specifications;
using Nexus.Erp.Domain.Entities.Catalog;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Procurement.Commands;
using NexusErp.Application.Procurement.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.Repository<Category>();
            var categorySpec = new CategoryByIdSpecification(request.CategoryId);
            var category = await categoryRepo.GetEntityWithSpecAsync(categorySpec);

            if (category == null) 
            {
                return Result.Failure<Guid>(new Error("Category.NotFound", "the choosen category not found"));
            }

            var productRepo = _unitOfWork.Repository<Product>();
            var barcodeSpec = new ProductByBarcodeSpecification(request.Barcode);
            var existingProduct = await productRepo.GetEntityWithSpecAsync(barcodeSpec);

            if (existingProduct != null) 
            {
                return Result.Failure<Guid>(new Error("Product.DuplicateBarcode", "The barcode is made for another product"));
            }

            var product = new Product
            {
                CategoryId = request.CategoryId,
                Name = request.Name,
                Barcode = request.Barcode,
                DefaultUnitCost = request.DefaultUnitCost,
                SellingPrice = request.SellingPrice,
            };

            await productRepo.AddItem(product);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(product.Id);
        }
    }
}
