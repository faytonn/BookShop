namespace Project.Api.Features.Coupons.Queries.GetCoupons;

public sealed class GetCouponsQueryHandler(ICouponService couponService) : IRequestHandler<GetCouponsQueryRequest, GetCouponsQueryResponse>
{
    public async Task<GetCouponsQueryResponse> Handle(GetCouponsQueryRequest query, CancellationToken cancellationToken)
    {
        var coupons = couponService.GetCoupons();
        return new GetCouponsQueryResponse(coupons);
    }
}

public sealed record GetCouponsQueryRequest() : IRequest<GetCouponsQueryResponse>;
public sealed record GetCouponsQueryResponse(IEnumerable<CouponResponse> Coupons);