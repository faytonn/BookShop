using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Repositories.Shared;

namespace Persistence.Repositories.Sellers;

public sealed class SellerRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Seller>> logger) : Repository<Seller>(context, contextAccessor, logger), ISellerRepository;
