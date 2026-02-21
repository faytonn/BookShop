namespace Application.CQRS.Coupons.Features.GetCoupons;

public sealed record GetCouponsQueryRequest() : IRequest<GetCouponsQueryResponse>;
public sealed record GetCouponsQueryResponse(IEnumerable<DTOs.CouponResponse> Coupons);

public sealed class GetCouponsQueryHandler(ICouponService couponService) : IRequestHandler<GetCouponsQueryRequest, GetCouponsQueryResponse>
{
    public async Task<GetCouponsQueryResponse> Handle(GetCouponsQueryRequest query, CancellationToken cancellationToken)
    {
        var coupons = couponService.GetCoupons();
        return new GetCouponsQueryResponse(coupons);
    }
}
