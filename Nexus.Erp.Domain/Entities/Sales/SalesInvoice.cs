using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Identity;
using Nexus.Erp.Domain.Entities.Organization;
using Nexus.Erp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Entities.Sales
{
    public class SalesInvoice : BaseEntity
    {
        public Guid BranchId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public decimal NetAmount => TotalAmount - DiscountAmount;
        public bool IsReturned { get; set; } = false;

        public PaymentStatus PaymentStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public Branch Branch { get; set; } = null!;
        public User User { get; set; } = null!;
    
        public ICollection<SalesInvoiceItem> SalesInvoiceItems { get; set; } = new List<SalesInvoiceItem>();
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
    }
}
