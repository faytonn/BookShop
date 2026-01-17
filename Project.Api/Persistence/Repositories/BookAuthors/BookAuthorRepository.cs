namespace Project.Api.Persistence.Repositories.BookAuthors;

public class BookAuthorRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<BookAuthor>> logger) : Repository<BookAuthor>(context, contextAccessor, logger), IBookAuthorRepository
{
}
