using Azure.Core;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Project.Api.Persistence.Repositories.Shared;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : BaseEntity
{
    private DbSet<T> Table => context.Set<T>();
    private CancellationToken _cancellation;
    private readonly ILogger<Repository<T>> _logger;

    public Repository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<T>> logger) : this(context)
    {
        _logger = logger;
        _cancellation = contextAccessor.HttpContext?.RequestAborted ?? default;
    }

    public void Add(T entity) => Table.Add(entity);
    public async ValueTask AddAsync(T entity)
    {

        using var reg = _cancellation.Register(() =>
            _logger.LogWarning("RequestAborted CANCELLED inside Repository<{Entity}>.AddAsync", typeof(T).Name));

        // simulate work to give you time to cancel the request:
        await Task.Delay(10_000, _cancellation);

        await Table.AddAsync(entity, _cancellation);
    }
    public void Update(T entity) => Table.Update(entity);
    public void Remove(T entity) => Table.Remove(entity);
    public int SaveChanges() => context.SaveChanges();
    public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync(_cancellation);

    public T? Find(Guid id) => Table.Find(id);
    public async Task<T?> FindAsync(Guid id) => await Table.FindAsync([id], _cancellation);
    public IQueryable<T> GetAll(bool tracking = false) => tracking ? Table : Table.AsNoTracking();
    public IQueryable<T> GetWhereAll(Expression<Func<T, bool>> filter) => GetAll().Where(filter);
}