using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Repositories.Shared;

namespace Persistence.Repositories.Authors;

public class AuthorRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Author>> logger) : Repository<Author>(context, contextAccessor, logger), IAuthorRepository
{

}
