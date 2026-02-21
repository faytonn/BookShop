namespace Application.Features.Coupons.Commands.UpdateCoupon;

public sealed class UpdateCouponCommandHandler(ICouponService couponService) : IRequestHandler<UpdateCouponCommandRequest, UpdateCouponCommandResponse>
{
    public async Task<UpdateCouponCommandResponse> Handle(UpdateCouponCommandRequest command, CancellationToken cancellationToken)
    {
        var response = await couponService.UpdateCouponAsync(command.CouponId, command.CouponRequest);

        if (response is null)
            throw new NotFoundException("Coupon not found.");

        return new UpdateCouponCommandResponse(response.Value.Id);
    }
}

public sealed record UpdateCouponCommandRequest(Guid CouponId, Application.CQRS.Coupons.DTOs.CouponRequest CouponRequest) : IRequest<UpdateCouponCommandResponse>;
public sealed record UpdateCouponCommandResponse(Guid CouponId);
