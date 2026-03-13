namespace Application.CQRS.Orders.Features.UpdateOrderStatus;

public sealed record UpdateOrderStatusCommandRequest(Guid OrderId, DTOs.UpdateOrderStatusRequest Request) : IRequest<UpdateOrderStatusCommandResponse>;
public sealed record UpdateOrderStatusCommandResponse(Guid OrderId, string DisplayCode, OrderStatus FromStatus, OrderStatus ToStatus, string? Description, DateTime ChangedAt);

public sealed class UpdateOrderStatusCommandHandler(IOrderService orderService) : IRequestHandler<UpdateOrderStatusCommandRequest, UpdateOrderStatusCommandResponse>
{
    public async Task<UpdateOrderStatusCommandResponse> Handle(UpdateOrderStatusCommandRequest command, CancellationToken cancellationToken)
    {
        var result = await orderService.UpdateOrderStatusAsync(command.OrderId, command.Request);

        return new UpdateOrderStatusCommandResponse(
            result.OrderId,
            result.DisplayCode,
            result.FromStatus,
            result.ToStatus,
            result.Description,
            result.ChangedAt
        );
    }
}
