using Microsoft.AspNetCore.Identity;
using Nexus.Erp.Domain.Entities.Identity;
using NexusErp.Application.Common.Permissions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace NexusErp.Infrastructure.Persistence.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAndPermissionsAsync(
            RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<ApplicationUser> userManager
            )
        {
            string[] roles = { AppRoles.Admin, AppRoles.Accountant, AppRoles.InventoryManager, AppRoles.Cashier };

            foreach(var roleName in roles)
            {
                if(!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }

            var adminRole = await roleManager.FindByNameAsync(AppRoles.Admin);
            if( adminRole is not null)
            {
                var adminClaims = await roleManager.GetClaimsAsync(adminRole);

                if(!adminClaims.Any(c => c.Value == Permissions.Users.Create))
                {
                    await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.Users.Create));
                    await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.Users.View));
                    await roleManager.AddClaimAsync(adminRole, new Claim("Permission", Permissions.Inventory.AddProduct));
                }
            }
        }
    }
}
