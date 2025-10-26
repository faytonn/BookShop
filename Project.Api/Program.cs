using Project.Api.Infrastucture.Extensions;
using Project.Api.Persistence.Extensions;
using Project.Api.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

builder.Services.AddPersistenceRegistrations();
builder.Services.AddInfrastructureRegistrations();
builder.Services.AddPresentationRegistrations(builder.Environment, builder.Configuration);

var app = builder.Build();

app.AddPresentationMiddlewares();

app.Run();