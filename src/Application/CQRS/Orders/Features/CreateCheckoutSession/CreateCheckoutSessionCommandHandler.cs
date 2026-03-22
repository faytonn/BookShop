namespace Application.CQRS.Orders.Features.CreateCheckoutSession;

public sealed record CreateCheckoutSessionCommandRequest(Guid OrderId) : IRequest<CreateCheckoutSessionCommandResponse>;
public sealed record CreateCheckoutSessionCommandResponse(Guid OrderId, string DisplayCode, string SessionId, string CheckoutUrl);

public sealed class CreateCheckoutSessionCommandHandler(IOrderService orderService)
    : IRequestHandler<CreateCheckoutSessionCommandRequest, CreateCheckoutSessionCommandResponse>
{
    public async Task<CreateCheckoutSessionCommandResponse> Handle(CreateCheckoutSessionCommandRequest command, CancellationToken cancellationToken)
    {
        var response = await orderService.CreateCheckoutSessionAsync(command.OrderId);

        return new CreateCheckoutSessionCommandResponse(
            response.OrderId,
            response.DisplayCode,
            response.SessionId,
            response.CheckoutUrl
        );
    }
}
