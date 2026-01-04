using Project.Api.Persistence.Repositories.Coupons;
using Project.Api.Persistence.Repositories.Languages;
using Project.Api.Persistence.Repositories.Orders;
using Project.Api.Persistence.Repositories.Sellers;
using Project.Api.Persistence.Repositories.Users;

namespace Project.Api.Persistence.UnitOfWorks;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
   public IBookLanguageRepository BookLanguages { get; }
    public IBookRepository Books { get; }
    public IBookSellerRepository BookSellers { get; }
    public ICategoryRepository Categories { get; }
    public ICouponRepository Coupons { get; }
    public ILanguageRepository Languages {  get; }
    public IOrderRepository Orders { get; }
    public ISellerRepository Sellers { get; }
    public IUserRepository Users {  get; }

    public UnitOfWork(
        AppDbContext context,
        IBookLanguageRepository bookLanguageRepository,
        IBookRepository bookRepository,
        IBookSellerRepository bookSellerRepository,
        ICategoryRepository categoryRepository,
        ICouponRepository couponRepository,
        ILanguageRepository languageRepository,
        IOrderRepository orderRepository,
        ISellerRepository sellerRepository,
        IUserRepository userRepository
    )
    {
        _context = context;
        BookLanguages = bookLanguageRepository;
        Books = bookRepository;
        BookSellers = bookSellerRepository;
        Categories = categoryRepository;
        Coupons = couponRepository;
        Languages = languageRepository;
        Orders = orderRepository;
        Sellers = sellerRepository;
        Users = userRepository;
    }

    public IDbContextTransaction? BeginTransaction() => _context.Database.BeginTransaction();

    public async Task<IDbContextTransaction?> BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();

    public void Commit() => _context.Database.CommitTransaction();

    public async Task CommitAsync() => await _context.Database.CommitTransactionAsync();

    public void Rollback() => _context.Database.RollbackTransaction();

    public async Task RollbackAsync() => await _context.Database.RollbackTransactionAsync();

    public int SaveChanges() => _context.SaveChanges();

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
