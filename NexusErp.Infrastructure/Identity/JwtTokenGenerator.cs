using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nexus.Erp.Domain.Entities.Identity;
using NexusErp.Application.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NexusErp.Infrastructure.Identity;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public JwtTokenGenerator(RoleManager<ApplicationRole> roleManager, IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<(string Token, string RefreshToken, DateTime Expiration)> GenerateTokensAsync(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(userClaims);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));

            var roleEntity = await _roleManager.FindByNameAsync(role);
            if (roleEntity != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
                claims.AddRange(roleClaims);
            }
        }

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["Secret"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expirationDays = double.Parse(jwtSettings["ExpirationInDays"] ?? "7");
        var expiration = DateTime.UtcNow.AddDays(expirationDays);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        var refreshToken = GenerateRefreshToken();

        return (accessToken, refreshToken, expiration);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}