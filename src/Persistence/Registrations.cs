using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Persistence;

public static class Registrations
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Data.AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"))
                /*.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning))*/;
            options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<UnitOfWorks.IUnitOfWork, UnitOfWorks.UnitOfWork>();

        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookAuthorRepository, BookAuthorRepository>();
        services.AddScoped<IBookLanguageRepository, BookLanguageRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookSellerRepository, BookSellerRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderHistoryRepository, OrderHistoryRepository>();
        services.AddScoped<ISellerRepository, SellerRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMetricRepository, MetricRepository>();

        services.AddScoped<ISaveChangesInterceptor, Data.Interceptors.AuditableEntityInterceptor>();

        return services;
    }
}