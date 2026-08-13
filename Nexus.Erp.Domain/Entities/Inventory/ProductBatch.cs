using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Catalog;
using Nexus.Erp.Domain.Entities.Organization;
using Nexus.Erp.Domain.Entities.Procurement;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nexus.Erp.Domain.Entities.Inventory
{
    public class ProductBatch : BaseEntity
    {
        public Guid BranchId { get; set; }
        public Guid ProductId { get; set; }
        public Guid GRNItemId { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public decimal QuantityAvailable { get; set; }
        public decimal UnitCost { get; set; }
        public decimal InitialQuantity { get; set; }
        public decimal TotalCost => QuantityAvailable * UnitCost;

        public Branch Branch { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public GRNItem GRNItem { get; set; } = null!;

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}