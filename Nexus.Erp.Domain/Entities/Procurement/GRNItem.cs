using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Catalog;
using Nexus.Erp.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Entities.Procurement
{
    public class GRNItem : BaseEntity
    {
        public Guid GRNId { get; set; }
        public Guid ProductId { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityRejected { get; set; }
        public decimal UnitCost { get; set; }
        public decimal QuantityAccepted => QuantityReceived - QuantityRejected;

        public GoodsReceiptNote GRN { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public ICollection<ProductBatch> productBatches { get; set; } = new List<ProductBatch>();
        
    }
}
