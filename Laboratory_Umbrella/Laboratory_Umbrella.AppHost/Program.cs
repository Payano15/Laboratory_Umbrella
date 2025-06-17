var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Laboratory_Umbrella_Api>("laboratory-umbrella-api");

builder.Build().Run();
