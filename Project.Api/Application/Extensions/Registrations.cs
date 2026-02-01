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

            services.AddMediatR(
                config => config.RegisterServicesFromAssembly(typeof(Program).Assembly)
            );

            services.AddHostedService<ExampleBackgroundService>();

            return services;
        }
    }
}
