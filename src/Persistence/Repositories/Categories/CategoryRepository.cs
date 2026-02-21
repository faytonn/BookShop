using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Repositories.Shared;

namespace Persistence.Repositories.Categories;

public sealed class CategoryRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Category>> logger) : Repository<Category>(context, contextAccessor, logger), ICategoryRepository;
