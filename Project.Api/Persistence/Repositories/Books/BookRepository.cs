using Project.Api.Persistence.Repositories.Books;
using Project.Api.Persistence.Repositories.Coupons;

public sealed class BookRepository(AppDbContext context) : Repository<Book>(context), IBookRepository;