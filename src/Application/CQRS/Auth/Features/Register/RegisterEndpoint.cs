namespace Application.CQRS.Auth.Features.Register;

public sealed class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/auth/register", Handler);
    }

    private static async Task<IResult> Handler(ISender sender, DTOs.RegisterRequest req)
    {
        await sender.Send(new RegisterCommandRequest(req));
        return Results.Created();
    }
}
