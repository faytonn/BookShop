using Project.Api.Persistence.Repositories.Coupons;

namespace Project.Api.Presentation.Extensions;

public static class PersistenceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistenceServices()
        {
            services.AddDbContext<AppDbContext>();
            services.AddScoped<ICouponRepository, CouponRepository>();

            return services;
        }
    }
}