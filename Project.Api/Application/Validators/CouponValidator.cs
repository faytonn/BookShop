namespace Project.Api.Application.Validators
{
    public class CouponValidator : AbstractValidator<CouponRequest>
    {
        public CouponValidator()
        {
            RuleFor(req => req.DiscountPercentage)
                .GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);

            RuleFor(req => req.ExpirationDate)
                .Must(date => date > DateTime.UtcNow.AddDays(2)).WithMessage("Coupon must be added for at least 2 days.");

            RuleFor(req => req.UsageLimit)
                .GreaterThan(0).LessThanOrEqualTo(1000000).WithMessage("Coupon limit should be greater than zero and less than the certain limit.");
        }
    }
}
