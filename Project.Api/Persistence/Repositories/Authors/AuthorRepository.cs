namespace Project.Api.Persistence.Repositories.Authors;

public class AuthorRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Author>> logger) : Repository<Author>(context, contextAccessor, logger), IAuthorRepository
{

}