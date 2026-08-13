using MediatR;
using Microsoft.AspNetCore.Identity;
using Nexus.Erp.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.DTOs.Auth;

namespace NexusErp.Application.Identity.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RefreshTokenCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.RefreshToken), cancellationToken);

        if (user is null)
        {
            throw new Exception("Invalid refresh token.");
        }

        var existingRefreshToken = user.RefreshTokens.Single(t => t.Token == request.RefreshToken);

        if (!existingRefreshToken.IsActive)
        {
            throw new Exception("Refresh token is expired or revoked.");
        }

        existingRefreshToken.RevokedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var (newToken, newRefreshToken, expiration) = await _jwtTokenGenerator.GenerateTokensAsync(user);

        var roles = (await _userManager.GetRolesAsync(user)).ToList();
        var userClaims = await _userManager.GetClaimsAsync(user);
        var permissions = userClaims.Select(c => c.Value).ToList();

        return new AuthResponse(
            user.Id.ToString(),
            user.Email!,
            newToken,
            newRefreshToken,
            expiration,
            roles,
            permissions
        );
    }
}