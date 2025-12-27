using System.Linq.Expressions;

namespace Project.Api.Persistence.Repositories.Shared;
public interface IRepository<T> where T : IEntity
{
    void Add(T entity);
    ValueTask AddAsync(T entity, CancellationToken cancellation);
    void Update(T entity);
    void Remove(T entity);
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellation);

    IQueryable<T> GetAll(bool tracking);
    IQueryable<T> GetWhereAll(Expression<Func<T, bool>> filter);
    T? Find(Guid id);
    Task<T?> FindAsync(Guid id, CancellationToken cancellation);
}