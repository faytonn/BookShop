using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Repositories.Shared;

namespace Persistence.Repositories.Coupons;

public sealed class CouponRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Coupon>> logger) : Repository<Coupon>(context, contextAccessor, logger), ICouponRepository
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
