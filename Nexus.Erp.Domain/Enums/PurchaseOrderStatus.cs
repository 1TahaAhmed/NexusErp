using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Enums
{
    public enum PurchaseOrderStatus
    {
        Pending = 0,
        Approved = 1,
        PartiallyReceived = 2,
        Completed = 3,
        Rejected = 4,
    }
}
