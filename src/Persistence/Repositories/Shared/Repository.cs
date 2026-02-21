using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using System.Linq.Expressions;

namespace Persistence.Repositories.Shared;

public class Repository<T>(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<T>> logger) : IRepository<T> where T : Entity
{
    private DbSet<T> Table => context.Set<T>();
    private CancellationToken _cancellation = contextAccessor.HttpContext?.RequestAborted ?? default;
    private readonly ILogger<Repository<T>> _logger = logger;

    public void Add(T entity) => Table.Add(entity);
    public async ValueTask AddAsync(T entity)=> await Table.AddAsync(entity, _cancellation);
    public void Update(T entity) => Table.Update(entity);
    public void Remove(T entity) => Table.Remove(entity);
    public int SaveChanges() => context.SaveChanges();
    public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync(_cancellation);

    public T? Find(Guid id) => Table.Find(id);
    public async Task<T?> FindAsync(Guid id) => await Table.FindAsync([id], _cancellation);
    public IQueryable<T> GetAll(bool tracking = false) => tracking ? Table : Table.AsNoTracking();
    public IQueryable<T> GetWhereAll(Expression<Func<T, bool>> filter) => GetAll().Where(filter);
    public async Task<IDbContextTransaction> BeginTransactionAsync()
       => await context.Database.BeginTransactionAsync(_cancellation);
}
