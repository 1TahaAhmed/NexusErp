using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace NexusErp.Application.Common.Permissions
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }

    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User.IsInRole(AppRoles.Admin))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var permissions = context.User.Claims
                .Where(x => x.Type == "Permission" && x.Value == requirement.Permission);

            if (permissions.Any())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}