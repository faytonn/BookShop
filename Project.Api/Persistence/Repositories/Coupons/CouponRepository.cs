namespace Project.Api.Persistence.Repositories.Coupons;

public sealed class CouponRepository(AppDbContext context, IHttpContextAccessor contextAccessor) : Repository<Coupon>(context), ICouponRepository
{
    private DbSet<Coupon> Coupons => context.Set<Coupon>();
    private CancellationToken cancellation = contextAccessor.HttpContext?.RequestAborted ?? default;

    public async Task<Coupon?> GetByIdAsync(Guid id) => 
       await Coupons
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellation);

    public async Task<Coupon?> GetByCodeAsync(string code)
    =>
      await Coupons
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.Code == code && !c.IsDeleted && c.IsActive,
                cancellation);

    public async Task<bool> CodeExistsAsync(string code)
    =>
      await Coupons
            .AnyAsync(c => c.Code == code, cancellation);

    public async Task AddRangeAsync(IEnumerable<Coupon> coupons)
    {
        await Coupons.AddRangeAsync(coupons, cancellation);
    }
}