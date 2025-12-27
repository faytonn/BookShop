using Project.Api.Application.Extensions;
using Project.Api.Infrastucture.Extensions;
using Project.Api.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);


builder.Services.AddHttpContextAccessor();

builder.Services
    .AddApplicationRegistrations()
    .AddPersistenceServices()
    .AddInfrastructureRegistrations()
    .AddPresentationRegistrations(builder.Environment, builder.Configuration);

var app = builder.Build();

app.AddPresentationMiddlewares();

app.Run();