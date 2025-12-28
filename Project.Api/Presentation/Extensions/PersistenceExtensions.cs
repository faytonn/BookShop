using Project.Api.Persistence.Repositories.BookLanguages;
using Project.Api.Persistence.Repositories.Books;
using Project.Api.Persistence.Repositories.Coupons;
using Project.Api.Persistence.Repositories.Languages;
using Project.Api.Persistence.Repositories.Orders;
using Project.Api.Persistence.Repositories.Sellers;
using Project.Api.Persistence.Repositories.Users;

namespace Project.Api.Presentation.Extensions;

public static class PersistenceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistenceServices()
        {
            services.AddDbContext<AppDbContext>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();


            return services;
        }
    }
}