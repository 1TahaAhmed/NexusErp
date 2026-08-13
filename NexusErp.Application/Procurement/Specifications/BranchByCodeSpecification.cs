using Nexus.Erp.Domain.Entities.Organization;
using NexusErp.Application.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Specifications
{
    public class BranchByCodeSpecification : BaseSpecification<Branch>
    {
        public BranchByCodeSpecification(string code)
            : base(b => b.Code == code)
        {
            
        }
    }
}
