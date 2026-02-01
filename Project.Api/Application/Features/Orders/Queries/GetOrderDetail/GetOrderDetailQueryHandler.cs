namespace Project.Api.Application.Features.Orders.Queries.GetOrderDetail;

public sealed class GetOrderDetailQueryHandler(IOrderService orderService) : IRequestHandler<GetOrderDetailQueryRequest, GetOrderDetailQueryResponse>
{
    public async Task<GetOrderDetailQueryResponse> Handle(GetOrderDetailQueryRequest query, CancellationToken cancellationToken)
    {
        var orderDetail = await orderService.GetOrderDetailAsync(query.Id);

        return new GetOrderDetailQueryResponse(orderDetail);
    }
}

public sealed record GetOrderDetailQueryRequest(Guid Id) : IRequest<GetOrderDetailQueryResponse>;
public sealed record GetOrderDetailQueryResponse(OrderDetailResponse Detail);