namespace Application.Features.Coupons.Commands.ActivateCoupon;

public sealed class ActivateCouponCommandHandler(ICouponService couponService) : IRequestHandler<ActivateCouponCommandRequest, ActivateCouponCommandResponse>
{
    public async Task<ActivateCouponCommandResponse> Handle(ActivateCouponCommandRequest command, CancellationToken cancellationToken)
    {
        var activated = await couponService.ActivateCouponAsync(command.CouponId);

        if (!activated)
            throw new NotFoundException("Coupon not found.");

        return new ActivateCouponCommandResponse();
    }
}

public sealed record ActivateCouponCommandRequest(Guid CouponId) : IRequest<ActivateCouponCommandResponse>;
public sealed record ActivateCouponCommandResponse();
