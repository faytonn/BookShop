using Project.Api.Persistence.Repositories.Books;
using Project.Api.Persistence.Repositories.Coupons;

public sealed class BookRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Book>> logger) : Repository<Book>(context, contextAccessor, logger), IBookRepository
{
    public IQueryable<Book> GetBooksWithCategories()
    {
        return context.Books.AsNoTracking()
                            .AsSplitQuery()
                            .Include(b => b.CategoryBooks);
    }

    public IQueryable<Book> GetBooksWithLanguagesThenSellers()
    {
        return context.Books.AsNoTracking()
                            .AsSplitQuery()
                            .Include(b => b.Languages)
                            .Include(b => b.BookSellers);
    }

    public IQueryable<Book> GetBooksWithSellers()
    {
        return context.Books.AsNoTracking()
                             .AsSplitQuery()
                             .Include(b => b.BookSellers);
    }
}