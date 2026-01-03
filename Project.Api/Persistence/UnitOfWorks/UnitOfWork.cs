namespace Project.Api.Persistence.UnitOfWorks;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IBookRepository Books { get; }
    public IBookSellerRepository BookSellers { get; }
    public IBookLanguageRepository BookLanguages { get; }

    public UnitOfWork(
        AppDbContext context,
        IBookRepository bookRepository,
        IBookSellerRepository bookSellerRepository,
        IBookLanguageRepository bookLanguageRepository
    )
    {
        _context = context;
        Books = bookRepository;
        BookSellers = bookSellerRepository;
        BookLanguages = bookLanguageRepository;
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
