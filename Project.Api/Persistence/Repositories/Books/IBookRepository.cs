namespace Project.Api.Persistence.Repositories.Books;

public interface IBookRepository : IRepository<Book>
{
    public IQueryable<Book> GetBooksWithCategories();
    public IQueryable<Book> GetBooksWithSellers();
    public IQueryable<Book> GetBooksWithLanguagesThenSellers();
}


