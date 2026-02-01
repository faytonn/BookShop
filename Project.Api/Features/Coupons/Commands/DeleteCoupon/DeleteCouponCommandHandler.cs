namespace Project.Api.Features.Coupons.Commands.DeleteCoupon;

public sealed class DeleteCouponCommandHandler(ICouponService couponService) : IRequestHandler<DeleteCouponCommandRequest, DeleteCouponCommandResponse>
{
    public async Task<DeleteCouponCommandResponse> Handle(DeleteCouponCommandRequest command, CancellationToken cancellationToken)
    {
        var deleted = await couponService.DeleteCouponAsync(command.CouponId);

        if (!deleted)
            throw new NotFoundException("Coupon not found or already deleted.");

        return new DeleteCouponCommandResponse();
    }
}

public sealed record DeleteCouponCommandRequest(Guid CouponId) : IRequest<DeleteCouponCommandResponse>;
public sealed record DeleteCouponCommandResponse();