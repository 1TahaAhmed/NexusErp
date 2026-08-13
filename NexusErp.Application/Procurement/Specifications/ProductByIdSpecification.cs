using Nexus.Erp.Domain.Entities.Catalog;
using NexusErp.Application.Common.Specifications;

namespace NexusErp.Application.Catalog.Specifications;

public class ProductByIdSpecification : BaseSpecification<Product>
{
    public ProductByIdSpecification(Guid id)
        : base(p => p.Id == id)
    {
    }
}