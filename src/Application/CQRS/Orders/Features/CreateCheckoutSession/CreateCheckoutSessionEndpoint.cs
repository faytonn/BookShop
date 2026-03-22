namespace Application.CQRS.Orders.Features.CreateCheckoutSession;

public sealed class CreateCheckoutSessionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/orders/{id:guid}/checkout-session", Handler)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handler(ISender sender, Guid id)
    {
        var response = await sender.Send(new CreateCheckoutSessionCommandRequest(id));
        return Results.Ok(response);
    }
}
