using Project.Api.Persistence.Repositories.BookLanguages;

public sealed class CategoryRepository(AppDbContext context, IHttpContextAccessor contextAccessor) : Repository<Category>(context, contextAccessor), ICategoryRepository;