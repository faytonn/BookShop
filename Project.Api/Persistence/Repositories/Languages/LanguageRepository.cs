using Project.Api.Persistence.Repositories.Languages;

public sealed class LanguageRepository(AppDbContext context) : Repository<Language>(context), ILanguageRepository;