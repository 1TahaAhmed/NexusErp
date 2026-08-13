using Nexus.Erp.Domain.Entities.Catalog;
using NexusErp.Application.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Specifications
{
    public class CategoryByNameSpecification : BaseSpecification<Category>
    {
        public CategoryByNameSpecification(string name)
            : base(c => c.Name.ToLower() == name.ToLower())
        {
            
        }
    }
}
