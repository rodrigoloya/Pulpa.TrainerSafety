using Microsoft.AspNetCore.Authorization;

namespace Pulpa.TrainerSafety.Api.Identity
{
    public class PermissionAuthorizationRequirement
        : AuthorizationHandler<PermissionAuthorizationRequirement>, IAuthorizationRequirement
    {
        public string[] AllowedPermissions { get; }

        public PermissionAuthorizationRequirement(params string[] allowedPermissions)
        {
            AllowedPermissions = allowedPermissions;
        }


        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            PermissionAuthorizationRequirement requirement)
        {
            foreach (var permission in requirement.AllowedPermissions)
            {
                if (context.User.HasClaim(c => c.Type == CustomClaimTypes.Permission && c.Value == permission))
                {
                    context.Succeed(requirement);
                    break;
                }
            }
            return Task.CompletedTask;
        }
    }
}