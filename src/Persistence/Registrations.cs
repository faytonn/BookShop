namespace Persistence;

public static class Registrations
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence(IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>((sp, opts) =>
            {
                opts.UseNpgsql(configuration.GetConnectionString("Postgres"));
                opts.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            });

            return services;
        }
    }
}