using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Catalog;
using Nexus.Erp.Domain.Entities.Organization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexus.Erp.Domain.Entities.Inventory
{
    public class BranchStock : BaseEntity
    {   
        public Guid BranchId { get; set; }
        public Guid ProductId { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal QuantityReserved { get; set; } = 0;
        public decimal ReorderLevel { get; set; } = 0;

        public decimal QuantityAvailable => QuantityOnHand - QuantityReserved;

        public Branch Branch { get; set; } = null!;
        public Product Product { get; set; } = null!;

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
