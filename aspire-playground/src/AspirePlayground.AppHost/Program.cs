var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var api = builder.AddProject<Projects.Api>("api")
                 .WithReference(cache);

builder.Build().Run();