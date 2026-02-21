namespace Application.CQRS.Coupons.Features.DeleteCoupon;

public sealed record DeleteCouponCommandRequest(Guid CouponId) : IRequest<DeleteCouponCommandResponse>;
public sealed record DeleteCouponCommandResponse();

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
