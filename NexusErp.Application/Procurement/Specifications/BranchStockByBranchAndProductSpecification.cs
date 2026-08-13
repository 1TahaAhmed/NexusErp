using Nexus.Erp.Domain.Entities.Inventory;
using NexusErp.Application.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Procurement.Specifications
{
    public class BranchStockByBranchAndProductSpecification : BaseSpecification<BranchStock>
    {
        public BranchStockByBranchAndProductSpecification(Guid branchId, Guid productId)
            : base(bs => bs.BranchId == branchId && bs.ProductId == productId)
        { }
    }
}
