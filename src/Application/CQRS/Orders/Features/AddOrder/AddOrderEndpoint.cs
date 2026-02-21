namespace Application.CQRS.Orders.Features.AddOrder;

public sealed class AddOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/orders", Handler)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handler(ISender sender, DTOs.AddOrderRequest request)
    {
        var response = await sender.Send(new AddOrderCommandRequest(request));
        return Results.Created($"/api/v1/orders/{response.Id}", response);
    }
}
