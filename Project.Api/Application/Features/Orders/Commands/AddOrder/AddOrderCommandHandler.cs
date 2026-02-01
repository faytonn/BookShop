namespace Project.Api.Application.Features.Orders.Commands.AddOrder;

public sealed class AddOrderCommandHandler(IOrderService orderService) : IRequestHandler<AddOrderCommandRequest, AddOrderCommandResponse>
{
    public async Task<AddOrderCommandResponse> Handle(AddOrderCommandRequest command, CancellationToken cancellationToken)
    {
        var orderResponse = await orderService.AddOrderAsync(command.Request);

        return new AddOrderCommandResponse(
            orderResponse.Id,
            orderResponse.TotalPrice,
            orderResponse.DisplayCode
        );
    }
}

public sealed record AddOrderCommandRequest(AddOrderRequest Request) : IRequest<AddOrderCommandResponse>;
public sealed record AddOrderCommandResponse(Guid Id, decimal TotalPrice, string DisplayCode);