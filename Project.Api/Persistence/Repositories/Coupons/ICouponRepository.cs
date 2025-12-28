namespace Project.Api.Persistence.Repositories.Coupons;

public interface ICouponRepository : IRepository<Coupon>
{
    Task<Coupon?> GetByIdAsync(Guid id);
    Task<Coupon?> GetByCodeAsync(string code);
    Task<bool> CodeExistsAsync(string code);
    Task AddRangeAsync(IEnumerable<Coupon> coupons);
}