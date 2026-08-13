using MediatR;
using Microsoft.AspNetCore.Identity;
using Nexus.Erp.Domain.Entities.Identity;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Text;

namespace NexusErp.Application.Identity.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            , IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("Invalid email or password.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                throw new Exception("Invalid email or password.");
            }

            var (token, refreshToken, expiration) = await _jwtTokenGenerator.GenerateTokensAsync(user);

            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(user);
            var permissions = userClaims.Select(c => c.Value).ToList();

            return new AuthResponse(
                user.Id.ToString(),
                user.Email ?? string.Empty,
                token,
                refreshToken,
                expiration,
                roles,
                permissions
            );
        }
    }
}
