using Microsoft.EntityFrameworkCore;
using Project.Api.Domain.Entities;

namespace Project.Api.Persistence.Contexts;

public sealed class AppDbContext(DbContextOptions<AppDbContext> opts, IConfiguration cfg) : DbContext(opts)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(cfg.GetConnectionString("Default"));
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<BookLanguage> BooksLanguages => Set<BookLanguage>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookLanguage>(e =>
        {
            e.HasKey(nameof(BookLanguage.BookId), nameof(BookLanguage.LanguageId));
        });
    }
}
