using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.DTOs.Auth
{
    public record AuthResponse(
        string UserId,
        string Email, 
        string Token,
        string RefreshToken,
        DateTime Expiration,
        List<string> Roles,
        List<string> Permissions
        );
}
