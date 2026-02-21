using Application.CQRS.Coupons.DTOs;

namespace Application.CQRS.Coupons.Features.UpdateCoupon;

public sealed record UpdateCouponCommandRequest(Guid CouponId, DTOs.CouponRequest CouponRequest) : IRequest<UpdateCouponCommandResponse>;
public sealed record UpdateCouponCommandResponse(Guid CouponId);

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
