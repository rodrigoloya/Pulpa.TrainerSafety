using Microsoft.EntityFrameworkCore;
using Pulpa.TrainerSafety.Api.AppStart;
using Pulpa.TrainerSafety.Api.Feature;
using Pulpa.TrainerSafety.Api.Identity;
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
        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    })  
);

builder.Services.AddIdentity<Pulpa.TrainerSafety.Data.Entities.UserTrainerSafety, 
    Microsoft.AspNetCore.Identity.IdentityRole>()
    .AddEntityFrameworkStores<Pulpa.TrainerSafety.Data.ApplicationDbContext>();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters.ValidIssuer = builder.Configuration["Jwt:Issuer"];
    opt.TokenValidationParameters.ValidAudience = builder.Configuration["Jwt:Audience"];
    opt.TokenValidationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? string.Empty));

});

builder.Services.AddAuthorization();


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.ApplyMigrations();
    await app.SeedRolesAndPermissions();
   
}
Registrations.MapEndpoints(app); 

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
