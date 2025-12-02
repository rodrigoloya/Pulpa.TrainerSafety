using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Pulpa_TrainerSafety_Api>("pulpa-trainersafety-api")
    .WithEnvironment("ConnectionStrings__DefaultConnection",
            builder.Configuration.GetConnectionString("DefaultConnection"))
    .WithEnvironment("Jwt__SecretKey", builder.Configuration.GetValue<string>("Jwt:SecretKey"))
    .WithExternalHttpEndpoints();

builder.Build().Run();
