using Project.Api.Persistence.Repositories.BookLanguages;

public sealed class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository;