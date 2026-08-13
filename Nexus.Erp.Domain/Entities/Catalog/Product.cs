using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Inventory;
using Nexus.Erp.Domain.Entities.Procurement;
using Nexus.Erp.Domain.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Entities.Catalog
{
    public class Product : BaseEntity
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public decimal DefaultUnitCost { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal ExpectedProfit => SellingPrice - DefaultUnitCost;
        public Category Category { get; set; } = null!;

        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
        public ICollection<GRNItem> GRNItems { get; set; } = new List<GRNItem>();
        public ICollection<BranchStock> BranchStocks { get; set; } = new List<BranchStock>();
        public ICollection<ProductBatch> ProductBatches { get; set; } = new List<ProductBatch>();
        public ICollection<SalesInvoiceItem> SalesInvoiceItems { get; set; } = new List<SalesInvoiceItem>();
    }
}
