using Project.Api.Persistence.Contexts;

namespace Project.Api.Persistence.Extensions;

public static class Registrations
{
    public static void AddPersistenceRegistrations(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
    }
}
