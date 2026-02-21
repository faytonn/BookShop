using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Persistence;

public static class Registrations
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Data.AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"))
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
        });

        services.AddScoped(typeof(Repositories.Shared.IRepository<>), typeof(Repositories.Shared.Repository<>));
        services.AddScoped<UnitOfWorks.IUnitOfWork, UnitOfWorks.UnitOfWork>();

        services.AddScoped<Repositories.Authors.IAuthorRepository, Repositories.Authors.AuthorRepository>();
        services.AddScoped<Repositories.BookAuthors.IBookAuthorRepository, Repositories.BookAuthors.BookAuthorRepository>();
        services.AddScoped<Repositories.BookLanguages.IBookLanguageRepository, Repositories.BookLanguages.BookLanguageRepository>();
        services.AddScoped<Repositories.Books.IBookRepository, Repositories.Books.BookRepository>();
        services.AddScoped<Repositories.BookSellers.IBookSellerRepository, Repositories.BookSellers.BookSellerRepository>();
        services.AddScoped<Repositories.Categories.ICategoryRepository, Repositories.Categories.CategoryRepository>();
        services.AddScoped<Repositories.Coupons.ICouponRepository, Repositories.Coupons.CouponRepository>();
        services.AddScoped<Repositories.Languages.ILanguageRepository, Repositories.Languages.LanguageRepository>();
        services.AddScoped<Repositories.Orders.IOrderRepository, Repositories.Orders.OrderRepository>();
        services.AddScoped<Repositories.Sellers.ISellerRepository, Repositories.Sellers.SellerRepository>();
        services.AddScoped<Repositories.Users.IUserRepository, Repositories.Users.UserRepository>();
        services.AddScoped<Repositories.Metrics.IMetricRepository, Repositories.Metrics.MetricRepository>();

        services.AddScoped<ISaveChangesInterceptor, Data.Interceptors.AuditableEntityInterceptor>();

        return services;
    }
}