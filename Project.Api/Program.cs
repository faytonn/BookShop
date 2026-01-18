
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();


builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

builder.Services.AddHttpContextAccessor();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services
    .AddApplicationRegistrations()
    .AddPersistenceServices()
    .AddInfrastructureRegistrations()
    .AddPresentationRegistrations(builder.Environment, builder.Configuration);

var app = builder.Build();

app.AddPresentationMiddlewares();

app.Run();