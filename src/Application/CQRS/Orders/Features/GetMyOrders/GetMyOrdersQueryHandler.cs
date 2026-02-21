namespace Application.CQRS.Orders.Features.GetMyOrders;

public sealed record GetMyOrdersQueryRequest() : IRequest<GetMyOrdersQueryResponse>;
public sealed record GetMyOrdersQueryResponse(IEnumerable<DTOs.MyOrdersResponse> MyOrders);

public sealed class GetMyOrdersQueryHandler(IOrderService orderService) : IRequestHandler<GetMyOrdersQueryRequest, GetMyOrdersQueryResponse>
{
    public async Task<GetMyOrdersQueryResponse> Handle(GetMyOrdersQueryRequest query, CancellationToken cancellationToken)
    {
        var myOrders = orderService.GetMyOrders();
        return new GetMyOrdersQueryResponse(myOrders);
    }
}
