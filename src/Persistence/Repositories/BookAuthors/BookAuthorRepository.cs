using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Repositories.Shared;

namespace Persistence.Repositories.BookAuthors;

public class BookAuthorRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<BookAuthor>> logger) : Repository<BookAuthor>(context, contextAccessor, logger), IBookAuthorRepository
{
}
