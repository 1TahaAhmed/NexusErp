using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        PartiallyPaid = 3,
        Refunded = 4,
        Failed = 5
    }
}
