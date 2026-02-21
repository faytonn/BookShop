using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Repositories.Shared;

namespace Persistence.Repositories.BookSellers
{
    public class BookSellerRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<BookSeller>> logger) : Repository<BookSeller>(context, contextAccessor, logger), IBookSellerRepository
    {
    }
}
