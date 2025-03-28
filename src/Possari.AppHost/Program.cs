var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Possari_WebApi>("webapi")
  .WithExternalHttpEndpoints();

builder.Build().Run();
