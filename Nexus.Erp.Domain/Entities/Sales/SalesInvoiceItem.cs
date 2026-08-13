using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Catalog;
using Nexus.Erp.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Entities.Sales
{
    public class SalesInvoiceItem : BaseEntity
    {
        public Guid SalesInvoiceId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductBatchId { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public SalesInvoice SalesInvoice { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public ProductBatch ProductBatch { get; set; } = null!;
    }
}
