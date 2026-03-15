using Application.CQRS.Orders.Features.UpdateOrderStatus;

namespace Application.CQRS.Orders.Features.CancelOrder;

public sealed record CancelOrderCommandRequest(CancelOrderRequest Request) : IRequest;

//public sealed record CancelOrderCommandResponse(CancelOrderResponse Response);

public class CancelOrderStatusCommandHandler(IOrderService orderService) : IRequestHandler<CancelOrderCommandRequest>
{
    public async Task Handle(CancelOrderCommandRequest command, CancellationToken cancellationToken)
    {
        await orderService.CancelOrderAsync(command.Request.OrderId);
    }
}
