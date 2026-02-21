namespace Persistence.Repositories.Books;

public interface IBookRepository : Repositories.Shared.IRepository<Book>
{
    public IQueryable<Book> GetBooksWithCategories();
    public IQueryable<Book> GetBooksWithSellers();
    public IQueryable<Book> GetBooksWithLanguagesThenSellers();
}

