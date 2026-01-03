
namespace Project.Api.Persistence.UnitOfWorks;

public interface IUnitOfWork : IDisposable
{
    IDbContextTransaction? BeginTransaction();
    Task<IDbContextTransaction?> BeginTransactionAsync();
    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
    int SaveChanges();
    Task<int> SaveChangesAsync();


    IBookRepository Books { get; }
    IBookSellerRepository BookSellers { get; }
    IBookLanguageRepository BookLanguages { get; }
}
