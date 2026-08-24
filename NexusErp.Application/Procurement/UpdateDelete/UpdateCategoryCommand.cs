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
    public record UpdateCategoryCommand(Guid Id, string Name, Guid? ParentCategoryId) : IRequest<Result<bool>>;

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<bool>>
    {
        private readonly IGenericRepository<Category> _categoryRepo;

        public UpdateCategoryCommandHandler(IGenericRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<Result<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepo.GetAllAsync();
            var category = categories.FirstOrDefault(c => c.Id == request.Id);

            if (category == null)
            {
                return Result.Failure<bool>(new Error("NotFound", "Category not found"));
            }

            category.Name = request.Name;
            category.ParentCategoryId = request.ParentCategoryId;

            _categoryRepo.UpdateItem(category);

            return Result<bool>.Success(true);
        }
    }
}