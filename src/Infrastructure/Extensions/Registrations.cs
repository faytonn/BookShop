using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Shared.Abstractions.Payments;
using Infrastructure.Providers.Payments.Stripe;

namespace Infrastructure.Extensions;

public static class Registrations
{
    public static IServiceCollection AddInfrastructureRegistrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<Providers.Tokens.TokenProvider>();
        services.AddScoped<Providers.Coupons.CouponGenerator>();
        var stripeOptions = new StripeOptions();
        configuration.GetSection("Stripe").Bind(stripeOptions);
        services.AddSingleton(stripeOptions);
        services.AddScoped<IPaymentService, StripePaymentService>();

        return services;
    }
}
