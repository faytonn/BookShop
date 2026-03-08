using Microsoft.EntityFrameworkCore.Design;

namespace Persistence.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Use a default connection string for migrations
        // This is only used at design time (when running migrations)
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=fatima_bookshop_db;Username=postgres;Password=postgres");
        
        return new AppDbContext(optionsBuilder.Options);
    }
}
