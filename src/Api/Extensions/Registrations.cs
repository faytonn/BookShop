using Persistence.Data;

namespace Api.Extensions;

public static class Registrations
{
    public static IServiceCollection AddPresentationRegistrations(this IServiceCollection services, IWebHostEnvironment env, IConfiguration cfg)
    {
        services.AddControllers();

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
        {
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg.GetValue<string>("Jwt:SecurityKey")!)),
                ValidIssuer = "",
                ValidAudience = "",
                ClockSkew = TimeSpan.Zero,
            };
        });
        services.AddAuthorization();

        services.AddCarter();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static void AddPresentationMiddlewares(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.MigrateAsync().GetAwaiter().GetResult();
        }

        //app.UseCustomExceptionHandler();

        app.UseExceptionHandler();

        if (app.Environment.IsProduction())
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}
