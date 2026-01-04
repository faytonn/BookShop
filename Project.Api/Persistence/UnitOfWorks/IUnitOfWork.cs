
using Project.Api.Persistence.Repositories.Coupons;
using Project.Api.Persistence.Repositories.Languages;
using Project.Api.Persistence.Repositories.Orders;
using Project.Api.Persistence.Repositories.Sellers;
using Project.Api.Persistence.Repositories.Users;

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
