namespace Project.Api.Features.Orders.Queries.GetAllOrders;

public sealed class GetAllOrdersQueryHandler(IOrderService orderService) : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
{
    public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest query, CancellationToken cancellationToken)
    {
        var orders = orderService.GetAllOrders();
        return new GetAllOrdersQueryResponse(orders);
    }
}

public sealed record GetAllOrdersQueryRequest() : IRequest<GetAllOrdersQueryResponse>;
public sealed record GetAllOrdersQueryResponse(IEnumerable<AllOrdersDBModel> Orders);