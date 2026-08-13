using Nexus.Erp.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.DTOs.Auth
{
    public record AuthResponseDto(
        string Token,
        string RefreshToken,
        DateTime RefreshTokenExpiration,
        string Email,
        List<string> Roles
    );
}
