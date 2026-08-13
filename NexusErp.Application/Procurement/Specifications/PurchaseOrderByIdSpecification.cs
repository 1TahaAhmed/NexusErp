using Nexus.Erp.Domain.Entities.Procurement;
using NexusErp.Application.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Specifications
{
    public class PurchaseOrderByIdSpecification : BaseSpecification<PurchaseOrder>
    {
        public PurchaseOrderByIdSpecification(Guid id)
            : base(po => po.Id == id)
        { }
    }
}
