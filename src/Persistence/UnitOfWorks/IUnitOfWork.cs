using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Repositories.Authors;
using Persistence.Repositories.BookAuthors;
using Persistence.Repositories.BookLanguages;
using Persistence.Repositories.Books;
using Persistence.Repositories.BookSellers;
using Persistence.Repositories.Categories;
using Persistence.Repositories.Coupons;
using Persistence.Repositories.Languages;
using Persistence.Repositories.Metrics;
using Persistence.Repositories.Orders;
using Persistence.Repositories.Sellers;
using Persistence.Repositories.Users;

namespace Persistence.UnitOfWorks;

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
    IMetricRepository Metrics { get; }


}
