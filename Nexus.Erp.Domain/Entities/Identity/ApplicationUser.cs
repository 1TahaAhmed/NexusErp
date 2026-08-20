using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        //  public string? RefreshToken { get; set; }
        // public DateTime? RefreshTokenExpiryTime { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        //public string UserName { get; set; } = string.Empty;

        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
