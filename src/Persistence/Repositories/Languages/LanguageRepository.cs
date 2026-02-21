using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Repositories.Shared;

namespace Persistence.Repositories.Languages;

public sealed class LanguageRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Language>> logger) : Repository<Language>(context, contextAccessor, logger), ILanguageRepository;
