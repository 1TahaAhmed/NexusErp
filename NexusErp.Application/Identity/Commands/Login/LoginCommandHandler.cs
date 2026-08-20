using MediatR;
using Microsoft.AspNetCore.Identity;
using Nexus.Erp.Domain.Entities.Identity;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.DTOs.Auth;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NexusErp.Application.Identity.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                user = _userManager.Users.FirstOrDefault(u => u.Email == request.Email);
            }

            if (user == null || user.PasswordHash == null)
            {
                return Result.Failure<AuthResponse>(new Error("Auth.InvalidCredentials", "User does not exist or has no password set."));
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return Result.Failure<AuthResponse>(new Error("Auth.InvalidCredentials", "Invalid password provided."));
            }

            var (token, refreshToken, expiration) = await _jwtTokenGenerator.GenerateTokensAsync(user);

            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(user);
            var permissions = userClaims.Select(c => c.Value).ToList();

            var authResponse = new AuthResponse(
                user.Id.ToString(),
                user.Email ?? string.Empty,
                token,
                refreshToken,
                expiration,
                roles,
                permissions
            );

            return Result.Success(authResponse);
        }
    }
}