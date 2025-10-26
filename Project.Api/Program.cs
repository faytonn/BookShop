using Microsoft.AspNetCore.Mvc;
using Project.Api;
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

builder.Services.AddSingleton<SingletonService>();
builder.Services.AddScoped<ScopedService>();
builder.Services.AddTransient<TransientService>();

var app = builder.Build();

app.AddPresentationMiddlewares();

app.MapGet("scopes", (
    [FromServices] SingletonService singletonService,
    [FromServices] ScopedService scopedService1,
    [FromServices] ScopedService scopedService2,
    [FromServices] TransientService transientService1,
    [FromServices] TransientService transientService2
    ) =>
{
    return Results.Ok(new {
        SingletonId = singletonService.Id,
        ScopedId1 = scopedService1.Id,
        ScopedId2 = scopedService2.Id,
        TransientId1 = transientService1.Id,
        TransientId2 = transientService2.Id
    });
});

app.Run();