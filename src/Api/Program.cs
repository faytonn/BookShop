var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPersistence(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.Run();