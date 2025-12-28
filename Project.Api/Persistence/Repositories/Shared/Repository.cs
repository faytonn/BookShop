using System.Linq.Expressions;

namespace Project.Api.Persistence.Repositories.Shared;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : BaseEntity
{
    private DbSet<T> Table => context.Set<T>();
    private static CancellationToken _cancellation;

    public static void Configure(IHttpContextAccessor contextAccessor)
    {
        _cancellation = contextAccessor.HttpContext?.RequestAborted ?? default;
    }

    public void Add(T entity) => Table.Add(entity);
    public async ValueTask AddAsync(T entity) => await Table.AddAsync(entity, _cancellation);
    public void Update(T entity) => Table.Update(entity);
    public void Remove(T entity) => Table.Remove(entity);
    public int SaveChanges() => context.SaveChanges();
    public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync(_cancellation);

    public T? Find(Guid id) => Table.Find(id);
    public async Task<T?> FindAsync(Guid id) => await Table.FindAsync([id], _cancellation);
    public IQueryable<T> GetAll(bool tracking = false) => tracking ? Table : Table.AsNoTracking();
    public IQueryable<T> GetWhereAll(Expression<Func<T, bool>> filter) => GetAll().Where(filter);
}