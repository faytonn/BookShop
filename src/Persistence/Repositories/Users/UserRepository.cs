using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Repositories.Shared;

namespace Persistence.Repositories.Users;

public sealed class UserRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<User>> logger) : Repository<User>(context, contextAccessor, logger), IUserRepository;
