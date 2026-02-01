namespace Project.Api.Features.Coupons.Queries.GetCouponByCode;

public sealed class GetCouponByCodeQueryHandler(ICouponService couponService) : IRequestHandler<GetCouponByCodeQueryRequest, GetCouponByCodeQueryResponse>
{
    public async Task<GetCouponByCodeQueryResponse> Handle(GetCouponByCodeQueryRequest query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query.Code))
            throw new ArgumentException($"Coupon code is required. Parameter: {nameof(query.Code)}");
        var coupon = await couponService.GetCouponByCodeAsync(query.Code);

        if (coupon is null)
            throw new NotFoundException($"Coupon with code {query.Code} not found.");

        return new GetCouponByCodeQueryResponse(coupon);
    }
}

public sealed record GetCouponByCodeQueryRequest(string Code) : IRequest<GetCouponByCodeQueryResponse>;
public sealed record GetCouponByCodeQueryResponse(CouponResponse? Coupon);