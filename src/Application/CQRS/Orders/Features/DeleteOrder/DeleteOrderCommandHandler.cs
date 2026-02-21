namespace Application.CQRS.Orders.Features.DeleteOrder;

public sealed record DeleteOrderCommandRequest(Guid Id) : IRequest;
public sealed record DeleteOrderCommandResponse();

public sealed class DeleteOrderCommandHandler(IOrderService orderService) : IRequestHandler<DeleteOrderCommandRequest>
{
    public async Task Handle(DeleteOrderCommandRequest command, CancellationToken cancellationToken)
    {
        await orderService.DeleteOrderAsync(command.Id);
    }
}
