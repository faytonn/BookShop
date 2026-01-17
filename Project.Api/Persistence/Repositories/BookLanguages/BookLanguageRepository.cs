namespace Project.Api.Persistence.Repositories.BookLanguages;

public sealed class BookLanguageRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<BookLanguage>> logger) : Repository<BookLanguage>(context, contextAccessor, logger), IBookLanguageRepository
{

}
