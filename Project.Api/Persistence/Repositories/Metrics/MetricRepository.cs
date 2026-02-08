namespace Project.Api.Persistence.Repositories.Metrics;

public sealed class MetricRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Metric>> logger) 
    : Repository<Metric>(context, contextAccessor, logger), IMetricRepository;
