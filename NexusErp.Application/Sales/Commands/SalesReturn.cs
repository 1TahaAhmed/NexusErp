using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Identity;
using Nexus.Erp.Domain.Entities.Organization;

namespace Nexus.Erp.Domain.Entities.Sales
{
    public class SalesReturn : BaseEntity
    {
        public Guid SalesInvoiceId { get; set; }
        public Guid BranchId { get; set; }
        public Guid CreatedByUserId { get; set; }

        public DateTime ReturnDate { get; set; } = DateTime.UtcNow;
        public decimal TotalRefundAmount { get; set; }
        public string? Reason { get; set; }

        public SalesInvoice SalesInvoice { get; set; } = null!;
        public Branch Branch { get; set; } = null!;

        [ForeignKey(nameof(CreatedByUserId))] 
        public User User { get; set; } = null!;

        public ICollection<SalesReturnItem> SalesReturnItems { get; set; } = new List<SalesReturnItem>();
    }
}