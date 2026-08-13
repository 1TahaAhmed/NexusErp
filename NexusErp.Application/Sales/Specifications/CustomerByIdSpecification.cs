using Nexus.Erp.Domain.Entities.Identity;
using NexusErp.Application.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Sales.Specifications
{
    public class CustomerByIdSpecification : BaseSpecification<User>
    {
        public CustomerByIdSpecification(Guid id)
            : base(c => c.Id == id)
        { }
    }
}
