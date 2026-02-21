using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Repositories.Shared;

namespace Persistence.Repositories.BookLanguages;

public sealed class BookLanguageRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<BookLanguage>> logger) : Repository<BookLanguage>(context, contextAccessor, logger), IBookLanguageRepository
{

}
