using Nexus.Erp.Domain.Entities.Inventory;
using NexusErp.Application.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Inventory
{
    public class AvailableBatchesFifoSpecification : BaseSpecification<ProductBatch>
    {
        public AvailableBatchesFifoSpecification(Guid branchId, Guid productId)
            : base(b => b.BranchId == branchId && b.ProductId == productId && b.QuantityAvailable > 0)
        {
            AddOrderBy(b => b.ExpiryDate);    
        }
    }
}
