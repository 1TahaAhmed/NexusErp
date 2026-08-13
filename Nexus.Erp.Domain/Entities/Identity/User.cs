using Nexus.Erp.Domain.Common;
using Nexus.Erp.Domain.Entities.Organization;
using Nexus.Erp.Domain.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Entities.Identity
{
    public class User : BaseEntity
    {
        public Guid BranchId { get; set; }
        public Guid RoleId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string? IdentityId { get; set; }

        public Branch Branch { get; set; } = null!;

        public Role Role { get; set; } = null!;

        public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
    }
}