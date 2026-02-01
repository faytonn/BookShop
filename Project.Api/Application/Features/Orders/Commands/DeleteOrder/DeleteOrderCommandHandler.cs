namespace Project.Api.Application.Features.Orders.Commands.DeleteOrder;

public sealed class DeleteOrderCommandHandler(IOrderService orderService) : IRequestHandler<DeleteOrderCommandRequest>
{
    public async Task Handle(DeleteOrderCommandRequest command, CancellationToken cancellationToken)
    {
        await orderService.DeleteOrderAsync(command.Id);
    }
}

public sealed record DeleteOrderCommandRequest(Guid Id) : IRequest;
public sealed record DeleteOrderCommandResponse();