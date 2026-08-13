using Nexus.Erp.Domain.Entities.Catalog;
using NexusErp.Application.Common.Specifications;

namespace Nexus.Erp.Application.Catalog.Specifications;

public class CategoryByIdSpecification : BaseSpecification<Category>
{
    public CategoryByIdSpecification(Guid id)
        : base(c => c.Id == id)
    {
    }
}