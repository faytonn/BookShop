namespace Application.CQRS.Orders.Features.AddOrder;

public sealed record AddOrderCommandRequest(DTOs.AddOrderRequest Request) : IRequest<AddOrderCommandResponse>;
public sealed record AddOrderCommandResponse(Guid Id, decimal TotalPrice, string DisplayCode);

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
