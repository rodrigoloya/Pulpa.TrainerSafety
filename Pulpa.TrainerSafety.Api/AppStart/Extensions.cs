using Microsoft.EntityFrameworkCore;

namespace Pulpa.TrainerSafety.Api.AppStart
{
    public static class Extensions
    {
        public static async Task ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<Pulpa.TrainerSafety.Data.ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
