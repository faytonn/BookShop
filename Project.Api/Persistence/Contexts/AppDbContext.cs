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
    public DbSet<CategoryBook> BooksCategories => Set<CategoryBook>();
    public DbSet<Coupon> Coupons => Set<Coupon>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookLanguage>(e =>
        {
            e.HasKey(nameof(BookLanguage.BookId), nameof(BookLanguage.LanguageId));
        });
        modelBuilder.Entity<CategoryBook>(e =>
        {
            e.HasKey(nameof(CategoryBook.BookId), nameof(CategoryBook.CategoryId));
        });


        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany()
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        

    }
}
