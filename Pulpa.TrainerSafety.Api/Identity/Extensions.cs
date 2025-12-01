using System.Security.Claims;

namespace Pulpa.TrainerSafety.Api.Identity
{  
    public static class Extensions
    {
        public static async Task SeedRolesAndPermissions(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole>>();
           
            var adminRole = await roleManager.FindByNameAsync(Roles.Admin);
            if(adminRole is null)
            {
                adminRole = new Microsoft.AspNetCore.Identity.IdentityRole(Roles.Admin);
                await roleManager.CreateAsync(adminRole);
                
                await roleManager.AddClaimAsync(adminRole,
                new Claim(CustomClaimTypes.Permission, Permissions.ReadUsers));
                await roleManager.AddClaimAsync(adminRole,
                     new Claim(CustomClaimTypes.Permission, Permissions.EditUsers));
                await roleManager.AddClaimAsync(adminRole,
                     new Claim(CustomClaimTypes.Permission, Permissions.DeleteUsers));
                await roleManager.AddClaimAsync(adminRole,
                     new Claim(CustomClaimTypes.Permission, Permissions.CreateUsers));
            }

            var memberRole = await roleManager.FindByNameAsync(Roles.Member);
            if(memberRole is null)
            {
                memberRole = new Microsoft.AspNetCore.Identity.IdentityRole(Roles.Member);
                await roleManager.CreateAsync(memberRole);
                await roleManager.AddClaimAsync(memberRole,
                new Claim(CustomClaimTypes.Permission, Permissions.ReadUsers));
            }
            
            if (!await roleManager.RoleExistsAsync(Roles.User))
            {
                await roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole(Roles.User));
            }
        }
    }
}
