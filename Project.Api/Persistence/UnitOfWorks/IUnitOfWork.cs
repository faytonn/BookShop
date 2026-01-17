using Project.Api.Persistence.Repositories.Authors;
using Project.Api.Persistence.Repositories.BookAuthors;

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


    IAuthorRepository Authors { get; }
    IBookAuthorRepository BookAuthors { get; }
    IBookLanguageRepository BookLanguages { get; }
    IBookRepository Books { get; }
    IBookSellerRepository BookSellers { get; }
    ICategoryRepository Categories { get; }
    ICouponRepository Coupons { get; }
    ILanguageRepository Languages {  get; }
    IOrderRepository Orders { get; }
    ISellerRepository Sellers { get; }
    IUserRepository Users {  get; }


}
