using MediatR;
using Nexus.Erp.Domain.Entities.Catalog;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Queries
{
    public record CategoryDto(
        Guid Id,
        string Name,
        Guid? ParentCategoryId
        );

    public record GetCategoriesListQuery() : IRequest<Result<IReadOnlyList<CategoryDto>>>;
    public class GetCategoriesListQueryHandler : IRequestHandler<GetCategoriesListQuery, Result<IReadOnlyList<CategoryDto>>>
    {
        private readonly IGenericRepository<Category> _categoryRepo;

        public GetCategoriesListQueryHandler(IGenericRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        public async Task<Result<IReadOnlyList<CategoryDto>>> Handle(GetCategoriesListQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepo.GetAllAsync();

            IReadOnlyList<CategoryDto> dtos = categories.Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.ParentCategoryId
            )).ToList();

            return Result<IReadOnlyList<CategoryDto>>.Success(dtos);
        }
    }
}
