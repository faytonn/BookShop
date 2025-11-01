using Project.Api.Infrastucture.Providers.Coupons;
using Project.Api.Infrastucture.Providers.Tokens;

namespace Project.Api.Infrastucture.Extensions;

public static class Registrations
{
    public static void AddInfrastructureRegistrations(this IServiceCollection services)
    {
        services.AddScoped<TokenProvider>();
        services.AddScoped<CouponGenerator>();
    }
}
