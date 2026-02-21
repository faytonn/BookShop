using Application.Services;
using Application.Services.Abstractions;
using Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class Registrations
{
    public static IServiceCollection AddApplicationRegistrations(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddScoped<IOrderService, OrderService>();

        services.AddMediatR(
            config => config.RegisterServicesFromAssembly(typeof(Registrations).Assembly)
        );

        services.AddHostedService<BackgroundServices.DailyBookMetricsBackgroundService>();

        return services;
    }
}
