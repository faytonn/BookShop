namespace Project.Api.Persistence.Repositories.BookSellers
{
    public class BookSellerRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<BookSeller>> logger) : Repository<BookSeller>(context, contextAccessor, logger), IBookSellerRepository
    {
    }
}
