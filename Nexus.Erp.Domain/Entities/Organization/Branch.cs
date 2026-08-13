using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Identity;
using Nexus.Erp.Domain.Entities.Inventory;
using Nexus.Erp.Domain.Entities.Procurement;
using Nexus.Erp.Domain.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Entities.Organization
{
    public class Branch : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<BranchStock> BranchStocks { get; set; } = new List<BranchStock>();
        public ICollection<ProductBatch> ProductBatches { get; set; } = new List<ProductBatch>();
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
        public ICollection<GoodsReceiptNote> GoodsReceiptNotes { get; set; } = new List<GoodsReceiptNote>();
        public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
    }
}
