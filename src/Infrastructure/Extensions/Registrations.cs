using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class Registrations
{
    public static IServiceCollection AddInfrastructureRegistrations(this IServiceCollection services)
    {
        services.AddTransient<Providers.Tokens.TokenProvider>();
        services.AddScoped<Providers.Coupons.CouponGenerator>();

        return services;
    }
}
