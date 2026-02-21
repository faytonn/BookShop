namespace Application.CQRS.Coupons.Features.CreateCoupon;

public sealed record CreateCouponCommandRequest(DTOs.CouponRequest CouponRequest) : IRequest<CreateCouponCommandResponse>;
public sealed record CreateCouponCommandResponse(Guid CouponId, string Code, DateTime ExpirationDate, int UsageLimit);

public sealed class CreateCouponCommandHandler(ICouponService couponService) : IRequestHandler<CreateCouponCommandRequest, CreateCouponCommandResponse>
{
    public async Task<CreateCouponCommandResponse> Handle(CreateCouponCommandRequest command, CancellationToken cancellationToken)
    {
        var response = await couponService.CreateCouponAsync(command.CouponRequest);

        return new CreateCouponCommandResponse(
            response.Id,
            response.Code,
            response.ExpirationDate,
            response.UsageLimit
        );
    }
}
