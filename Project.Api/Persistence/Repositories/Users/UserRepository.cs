using Project.Api.Persistence.Repositories.Users;

public sealed class UserRepository(AppDbContext context, IHttpContextAccessor contextAccessor) : Repository<User>(context, contextAccessor), IUserRepository;