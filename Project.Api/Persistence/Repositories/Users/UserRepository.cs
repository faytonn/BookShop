using Project.Api.Persistence.Repositories.Users;

public sealed class UserRepository(AppDbContext context) : Repository<User>(context), IUserRepository;