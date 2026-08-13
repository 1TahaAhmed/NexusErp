using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Entities.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string Description { get; set; } = string.Empty;

        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName)
        { }
    }
}
