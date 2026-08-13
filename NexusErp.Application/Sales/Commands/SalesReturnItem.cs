using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Catalog;
using System;

namespace Nexus.Erp.Domain.Entities.Sales
{
    public class SalesReturnItem : BaseEntity
    {
        public Guid SalesReturnId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public SalesReturn SalesReturn { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}