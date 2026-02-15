namespace Persistence;

public static class Registrations
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence(IConfiguration configuration)
        {
            return services;
        }
    }
}