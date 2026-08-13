using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Organization;
using Nexus.Erp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Entities.Procurement
{
    public class PurchaseOrder : BaseEntity
    {
        public Guid SupplierId { get; set; }
        public Guid BranchId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Pending;
        public Supplier Supplier { get; set; } = null!;
        public Branch Branch { get; set; } = null!;
        public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
        public ICollection<GoodsReceiptNote> GoodsReceiptNotes { get; set; } = new List<GoodsReceiptNote>();
    }
}
