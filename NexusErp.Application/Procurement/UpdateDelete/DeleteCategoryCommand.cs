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
    public record DeleteCategoryCommand(Guid Id) : IRequest<Result<bool>>;

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<bool>>
    {
        private readonly IGenericRepository<Category> _categoryRepo;

        public DeleteCategoryCommandHandler(IGenericRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepo.GetAllAsync();
            var category = categories.FirstOrDefault(c => c.Id == request.Id);

            if (category == null)
            {
                return Result.Failure<bool>(new Error("NotFound", "Category not found"));
            }

            _categoryRepo.DeleteItem(category);

            return Result<bool>.Success(true);
        }
    }
}