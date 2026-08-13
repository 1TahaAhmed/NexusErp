using MediatR;
using Nexus.Erp.Domain.Entities.Catalog;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Procurement.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.Repository<Category>();

            var spec = new CategoryByNameSpecification(request.Name);
            var existingCategory = await categoryRepo.GetEntityWithSpecAsync(spec);

            if (existingCategory != null) 
            {
                return Result.Failure<Guid>(new Error("Category.DuplicateName", "The name is already exist!"));
            }

            var category = new Category
            {
                Name = request.Name,
                ParentCategoryId = request.ParentCategoryId,
            };

            await categoryRepo.AddItem(category);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(category.Id);
        }
    }
}