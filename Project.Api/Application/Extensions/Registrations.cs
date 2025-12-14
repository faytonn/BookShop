using Project.Api.Application.Services;
using Project.Api.Application.Services.Abstractions;
using Project.Api.Application.Services.Implementations;

namespace Project.Api.Application.Extensions
{
    public static class Registrations
    {
        public static IServiceCollection AddApplicationRegistrations(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
