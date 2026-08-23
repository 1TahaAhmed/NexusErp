using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nexus.Erp.Domain.Entities.Identity;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Application.Common.Models;
using NexusErp.Application.Common.Permissions;
using NexusErp.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusErp.Application.Identity.Commands.Registeration
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RegisterCommandHandler(
            UserManager<ApplicationUser> userManager
            , IJwtTokenGenerator jwtTokenGenerator,
            RoleManager<ApplicationRole> roleManager
            )
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleManager = roleManager;
        }
        public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return Result.Failure<AuthResponse>(new Error("Auth.InvalidEmail", "Your Email is required!"));
            }

            var existingUserName = await _userManager.FindByNameAsync(request.UserName.Trim());
            if (existingUserName != null)
            {
                return Result.Failure<AuthResponse>(new Error("Auth.UserNameExists", "User's UserName is already used."));
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email.Trim());
            if (existingUser != null) 
            {
                return Result.Failure<AuthResponse>(new Error("Auth.EmailExists", "User's Email is already Exists"));
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var firstError = createResult.Errors.FirstOrDefault()?.Description ?? "Failed to create account";
                return Result.Failure<AuthResponse>(new Error("Auth.RegistrationFailed", firstError));
            }

            if(!await _roleManager.RoleExistsAsync(request.RoleName))
            {
                return Result.Failure<AuthResponse>(new Error("Auth.InvalidRole", "Selected role does not exist."));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, request.RoleName);
            if (!roleResult.Succeeded)
            {
                var firstError = createResult.Errors.FirstOrDefault()?.Description ?? "Failed to create account";
                return Result.Failure<AuthResponse>(new Error("Auth.RegistrationFailed", firstError));
            }

            var (token, refreshToken, expiration) = await _jwtTokenGenerator.GenerateTokensAsync(user);


            var roles = (await _userManager.GetRolesAsync(user)).ToList();

            var permissions = new List<string>();
            foreach (var roleName in roles) 
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null) 
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    permissions.AddRange(roleClaims.Select(c => c.Value));
                }
            }

            var userClaims = (await _userManager.GetClaimsAsync(user)).ToList();
            permissions.AddRange(userClaims.Select(c => c.Value));

            permissions = permissions.Distinct().ToList();

            var authResponse = new AuthResponse(
                user.Id.ToString(),
                user.Email,
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
