namespace Application.Features.Coupons.Commands.CreateCoupon;

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

public sealed record CreateCouponCommandRequest(CouponRequest CouponRequest) : IRequest<CreateCouponCommandResponse>;
public sealed record CreateCouponCommandResponse(Guid CouponId, string Code, DateTime ExpirationDate, int UsageLimit);
