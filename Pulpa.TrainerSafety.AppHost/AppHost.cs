var builder = DistributedApplication.CreateBuilder(args);



builder.AddProject<Projects.Pulpa_TrainerSafety_Api>("pulpa-trainersafety-api");

builder.Build().Run();
