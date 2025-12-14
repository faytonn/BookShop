using Project.Api.Infrastucture.Providers.Coupons;
using Project.Api.Infrastucture.Providers.Tokens;

namespace Project.Api.Infrastucture.Extensions;

public static class Registrations
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructureRegistrations()
        {
            services.AddTransient<TokenProvider>();
            services.AddScoped<CouponGenerator>();
        }
    }
}
