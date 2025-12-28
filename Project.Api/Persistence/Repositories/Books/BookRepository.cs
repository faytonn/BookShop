using Project.Api.Persistence.Repositories.Books;
using Project.Api.Persistence.Repositories.Coupons;

public sealed class BookRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Book>> logger) : Repository<Book>(context, contextAccessor, logger), IBookRepository;