
using Pulpa.TrainerSafety.Data;

namespace Pulpa.TrainerSafety.Api.Feature
{
    public static class RegisterUser
    {
        public record Request(string Email, string FirstName, string LastName, string Password, bool EnableNotifications= true);
    
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("ping-register-user", () => "Pong from Register User");
            app.MapPost("/register-user", async (Request request,ApplicationDbContext dbContext, UserManager<UserTrainerSafety> userManager, IPasswordHasher<UserTrainerSafety> passwordHasher) =>
            {
                using var transaction = await dbContext.Database.BeginTransactionAsync();
                var newUser = new UserTrainerSafety
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    EnableNotifications = request.EnableNotifications,
                    SubscriptionType = SubscriptionType.Free,
                    Status = UserTrainerSafetyStatus.Active,
                    CreatedAt = DateTime.UtcNow
                };
                     
                var hashedPassword = passwordHasher.HashPassword(newUser, request.Password);
                newUser.PasswordHash = hashedPassword;

                var identityResult = await userManager.CreateAsync(newUser, request.Password);

                if (!identityResult.Succeeded)
                {
                    var errors = identityResult.Errors.Select(e => e.Description);
                    return Results.BadRequest(new { Errors = errors });
                }
               
                var addToRoleResult = await userManager.AddToRoleAsync(newUser, Roles.User);

                if (!addToRoleResult.Succeeded)
                {
                    var errors = addToRoleResult.Errors.Select(e => e.Description);
                    return Results.BadRequest(new { Errors = errors });
                }

                await transaction.CommitAsync();
                //TODO: await emailService.SendWelcomeEmailAsync(newUser.Email, newUser.FirstName);

                return Results.Created($"/users/{newUser.Id}", new { newUser.Id, newUser.Email, newUser.FirstName, newUser.LastName });
            });
        }
    }
}
