using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Entities.Procurement
{
    public class PurchaseOrderItem : BaseEntity
    {
        public Guid PurchaseOrderId { get; set; }
        public Guid ProductId { get; set; }
        public decimal QuantityOrdered { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost => QuantityOrdered * UnitCost;
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
