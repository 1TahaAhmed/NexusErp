using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Entities.Procurement
{
    public class GoodsReceiptNote : BaseEntity
    {
        public Guid PurchaseOrderId { get; set; }
        public Guid SupplierId { get; set; }
        public Guid BranchId { get; set; }
        public DateTime ReceivedDate { get; set; } = DateTime.UtcNow;
        public string InvoiceNumber { get; set; } = string.Empty;
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        public Supplier Supplier { get; set; } = null!;
        public Branch Branch { get; set; } = null!;
        public ICollection<GRNItem> GRNItems { get; set; } = new List<GRNItem>();
    }
}
