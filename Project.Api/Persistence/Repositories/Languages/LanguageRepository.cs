using Project.Api.Persistence.Repositories.Languages;

public sealed class LanguageRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Language>> logger) : Repository<Language>(context, contextAccessor, logger), ILanguageRepository;