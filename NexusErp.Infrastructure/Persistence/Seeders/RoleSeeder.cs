using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Nexus.Erp.Domain.Entities.Identity;
using NexusErp.Application.Common.Permissions;
using System.Collections.Generic;
using System.Security.Claims;

namespace NexusErp.Infrastructure.Persistence.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAndPermissionsAsync(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            string[] roles = { AppRoles.Admin, AppRoles.Accountant, AppRoles.InventoryManager, AppRoles.Cashier };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole(roleName));
                }
            }

            await SeedRolePermissionsAsync(roleManager);

            var adminSection = configuration.GetSection("InitialAdmin");
            
            var adminEmail = adminSection["Email"] ?? throw new InvalidOperationException("InitialAdmin:Email is missing in configuration.");
            var adminPassword = adminSection["Password"] ?? throw new InvalidOperationException("InitialAdmin:Password is missing in configuration.");
            
            var defaultAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (defaultAdmin == null)
            {
                defaultAdmin = new ApplicationUser
                {
                    FirstName = adminSection["FirstName"] ?? "Admin",
                    LastName = adminSection["LastName"] ?? "System",
                    UserName = adminSection["UserName"] ?? "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(defaultAdmin, adminPassword);
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultAdmin, AppRoles.Admin);
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(defaultAdmin, AppRoles.Admin))
                {
                    await userManager.AddToRoleAsync(defaultAdmin, AppRoles.Admin);
                }
            }
        }

        private static async Task SeedRolePermissionsAsync(RoleManager<ApplicationRole> roleManager)
        {
            var cashierRole = await roleManager.FindByNameAsync(AppRoles.Cashier);
            if (cashierRole != null)
            {
                var cashierPermissions = new[]
                {
                    Permissions.Sales.CreateInvoice,
                    Permissions.Sales.View,
                    Permissions.Products.View
                };

                var existingClaims = await roleManager.GetClaimsAsync(cashierRole);
                foreach (var permission in cashierPermissions)
                {
                    if (!existingClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                    {
                        await roleManager.AddClaimAsync(cashierRole, new Claim("Permission", permission));
                    }
                }
            }
        }
    }
}