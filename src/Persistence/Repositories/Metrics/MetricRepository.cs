using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Repositories.Shared;

namespace Persistence.Repositories.Metrics;

public sealed class MetricRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Metric>> logger) 
    : Repository<Metric>(context, contextAccessor, logger), IMetricRepository;
