namespace Project.Api.Features.Coupons.Commands.DeactivateCoupon;

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

public sealed record DeactivateCouponCommandRequest(Guid CouponId) : IRequest<DeactivateCouponCommandResponse>;
public sealed record DeactivateCouponCommandResponse();