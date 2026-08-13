using Nexus.Erp.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        Task<(string Token, string RefreshToken, DateTime Expiration)> GenerateTokensAsync(ApplicationUser user); 
    }
}
