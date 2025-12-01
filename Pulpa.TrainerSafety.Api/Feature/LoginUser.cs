using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Pulpa.TrainerSafety.Api.Identity;
using Pulpa.TrainerSafety.Data;
using System.Text;

namespace Pulpa.TrainerSafety.Api.Feature;

public static class LoginUser
{
    public record Request (string Email, string Password);

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/me", (System.Security.Claims.ClaimsPrincipal cp) => {
            var result = cp.Claims.GroupBy(c=> c.Type)
            .ToDictionary(
                c => c.Key,
                c=> string.Join(',', c.Select(claim=> claim.Value )));
            return Results.Ok(result);
        }). RequireAuthorization( policy =>
        {
            policy.RequirePermission(Permissions.DeleteUsers, Permissions.EditUsers);
        });

        app.MapPost("/login", async (Request request, 
            UserManager<UserTrainerSafety> userManager,
            ApplicationDbContext dbContext,
            IConfiguration configuration) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null ||
                !await userManager.CheckPasswordAsync(user, request.Password))
            {
                return Results.Unauthorized();
            }

            string accessToken = await GenerateJwt(userManager, configuration,dbContext, user);

            return Results.Ok(new { AccessToken = accessToken });
        })
        .WithTags("Authentication")
        .WithName("LoginUser")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }

    private static async Task<string> GenerateJwt(UserManager<UserTrainerSafety> userManager, IConfiguration configuration, ApplicationDbContext dbContext, UserTrainerSafety user)
    {
        var roles = await userManager.GetRolesAsync(user);

        //secret key, issuer, audience, expiration from appsettings
        var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"] ?? string.Empty));

        var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
            signingKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

        var permissions = await (from role in dbContext.Roles
                           join claim in dbContext.RoleClaims on role.Id equals claim.RoleId
                           where roles.Contains(role.Name) &&
                           claim.ClaimType == CustomClaimTypes.Permission
                           select claim.ClaimValue)
                             .Distinct()
                             .ToArrayAsync();

        List<System.Security.Claims.Claim> claims = [
            new( JwtRegisteredClaimNames.Sub, user.Id),
                new( JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                //passing a list inside of a list [] 😮
                ..roles.Select(role => new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role)),
                ..permissions.Select(p => new System.Security.Claims.Claim(CustomClaimTypes.Permission, p))
                ];

        var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = credentials
        };

        var tokenHandler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
        string accessToken = tokenHandler.CreateToken(tokenDescriptor);
        return accessToken;
    }
}
