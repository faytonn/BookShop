namespace Application.CQRS.Orders.Features.GetOrderDetail;

public sealed record GetOrderDetailQueryRequest(Guid Id) : IRequest<GetOrderDetailQueryResponse>;
public sealed record GetOrderDetailQueryResponse(DTOs.OrderDetailResponse Detail);

public sealed class GetOrderDetailQueryHandler(IOrderService orderService) : IRequestHandler<GetOrderDetailQueryRequest, GetOrderDetailQueryResponse>
{
    public async Task<GetOrderDetailQueryResponse> Handle(GetOrderDetailQueryRequest query, CancellationToken cancellationToken)
    {
        var orderDetail = await orderService.GetOrderDetailAsync(query.Id);

        return new GetOrderDetailQueryResponse(orderDetail);
    }
}
