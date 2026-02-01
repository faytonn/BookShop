namespace Project.Api.Application.Features.Coupons.Queries.GetCouponById;

public sealed class GetCouponByIdQueryHandler(ICouponService couponService) : IRequestHandler<GetCouponByIdQueryRequest, GetCouponByIdQueryResponse>
{
    public async Task<GetCouponByIdQueryResponse> Handle(GetCouponByIdQueryRequest query, CancellationToken cancellationToken)
    {
        var coupon = await couponService.GetCouponAsync(query.CouponId);

        if (coupon is null)
            throw new NotFoundException("Coupon not found.");

        return new GetCouponByIdQueryResponse(coupon);
    }
}

public sealed record GetCouponByIdQueryRequest(Guid CouponId) : IRequest<GetCouponByIdQueryResponse>;
public sealed record GetCouponByIdQueryResponse(CouponResponse? Coupon);