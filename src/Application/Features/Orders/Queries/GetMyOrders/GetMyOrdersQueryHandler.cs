namespace Application.Features.Orders.Queries.GetMyOrders;

public sealed class GetMyOrdersQueryHandler(IOrderService orderService) : IRequestHandler<GetMyOrdersQueryRequest, GetMyOrdersQueryResponse>
{
    public async Task<GetMyOrdersQueryResponse> Handle(GetMyOrdersQueryRequest query, CancellationToken cancellationToken)
    {
        var myOrders = orderService.GetMyOrders();
        return new GetMyOrdersQueryResponse(myOrders);
    }
}

public sealed record GetMyOrdersQueryRequest() : IRequest<GetMyOrdersQueryResponse>;
public sealed record GetMyOrdersQueryResponse(IEnumerable<MyOrdersResponse> MyOrders);
