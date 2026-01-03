using Project.Api.Persistence.Repositories.Users;

public sealed class UserRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<User>> logger) : Repository<User>(context, contextAccessor, logger), IUserRepository;