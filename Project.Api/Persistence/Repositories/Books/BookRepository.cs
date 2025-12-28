using Project.Api.Persistence.Repositories.Books;
using Project.Api.Persistence.Repositories.Coupons;

public sealed class BookRepository(AppDbContext context, IHttpContextAccessor contextAccessor) : Repository<Book>(context, contextAccessor), IBookRepository;