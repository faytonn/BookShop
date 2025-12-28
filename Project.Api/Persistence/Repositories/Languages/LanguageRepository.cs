using Project.Api.Persistence.Repositories.Languages;

public sealed class LanguageRepository(AppDbContext context, IHttpContextAccessor contextAccessor) : Repository<Language>(context, contextAccessor), ILanguageRepository;