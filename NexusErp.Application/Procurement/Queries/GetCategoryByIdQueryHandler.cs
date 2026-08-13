using MediatR;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using Nexus.Erp.Domain.Entities.Catalog;

namespace NexusErp.Application.Procurement.Queries;

public record GetCategoryByIdQuery(Guid Id) : IRequest<Result<CategoryDto>>;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
    private readonly IGenericRepository<Category> _categoryRepo;

    public GetCategoryByIdQueryHandler(IGenericRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepo.GetAllAsync();
        var category = categories.FirstOrDefault(c => c.Id == request.Id);

        if (category == null)
            return Result.Failure<CategoryDto>(new Error("NotFound","Category not found"));

        var dto = new CategoryDto(category.Id, category.Name, category.ParentCategoryId);
        return Result<CategoryDto>.Success(dto);
    }
}