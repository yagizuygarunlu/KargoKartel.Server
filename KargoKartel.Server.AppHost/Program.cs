var builder = DistributedApplication.CreateBuilder(args);

// Reference the WebAPI project
builder.AddProject<Projects.KargoKartel_Server_WebAPI>("webapi");

builder.Build().Run();
