namespace Application.CQRS.Orders.Features.GetAllOrders;

public sealed record GetAllOrdersQueryRequest() : IRequest<GetAllOrdersQueryResponse>;
public sealed record GetAllOrdersQueryResponse(IEnumerable<DTOs.AllOrdersDBModel> Orders);

public sealed class GetAllOrdersQueryHandler(IOrderService orderService) : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
{
    public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest query, CancellationToken cancellationToken)
    {
        var orders = orderService.GetAllOrders();
        return new GetAllOrdersQueryResponse(orders);
    }
}
