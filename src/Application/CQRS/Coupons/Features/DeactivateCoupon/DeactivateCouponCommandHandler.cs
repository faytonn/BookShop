namespace Application.CQRS.Coupons.Features.DeactivateCoupon;

public sealed record DeactivateCouponCommandRequest(Guid CouponId) : IRequest<DeactivateCouponCommandResponse>;
public sealed record DeactivateCouponCommandResponse();

public sealed class DeactivateCouponCommandHandler(ICouponService couponService) : IRequestHandler<DeactivateCouponCommandRequest, DeactivateCouponCommandResponse>
{
    public async Task<DeactivateCouponCommandResponse> Handle(DeactivateCouponCommandRequest command, CancellationToken cancellationToken)
    {
        var deactivated = await couponService.DeactivateCouponAsync(command.CouponId);

        if (!deactivated)
            throw new NotFoundException("Coupon not found.");

        return new DeactivateCouponCommandResponse();
    }
}
