using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Entities.Sales
{
    public class PaymentTransaction : BaseEntity
    {
        public Guid SalesInvoiceId { get; set; }
        public string GatewayProvider { get; set; } = string.Empty;
        public string TransactionReference { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string RawResponse { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }

        public SalesInvoice SalesInvoice { get; set; } = null!;
    }
}
