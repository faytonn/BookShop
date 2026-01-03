using Project.Api.Persistence.Repositories.BookLanguages;

public sealed class CategoryRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Category>> logger) : Repository<Category>(context, contextAccessor, logger), ICategoryRepository;