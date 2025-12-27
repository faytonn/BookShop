namespace Project.Api.Persistence.Repositories.Coupons;

public sealed class CouponRepository(AppDbContext context) : Repository<Coupon>(context), ICouponRepository;