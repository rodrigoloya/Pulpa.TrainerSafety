using Microsoft.EntityFrameworkCore;
using Pulpa.TrainerSafety.Api.Feature;
using Pulpa.TrainerSafety.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<Pulpa.TrainerSafety.Data.ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    })
  
);

builder.Services.AddIdentity<Pulpa.TrainerSafety.Data.Entities.UserTrainerSafety, 
    Microsoft.AspNetCore.Identity.IdentityRole>()
    .AddEntityFrameworkStores<Pulpa.TrainerSafety.Data.ApplicationDbContext>();


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<Pulpa.TrainerSafety.Data.ApplicationDbContext>();
    dbContext.Database.Migrate();

    var roleManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole>>();
    if(!await roleManager.RoleExistsAsync(Roles.Admin))
    {
        await roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole(Roles.Admin));
    }
    if (!await roleManager.RoleExistsAsync(Roles.Member))
    {
        await roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole(Roles.Member));
    }
    if (!await roleManager.RoleExistsAsync(Roles.User))
    {
        await roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole(Roles.User));
    }
}

RegisterUser.MapEndpoint(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
