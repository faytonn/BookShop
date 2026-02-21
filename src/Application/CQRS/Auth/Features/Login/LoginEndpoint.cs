namespace Application.CQRS.Auth.Features.Login;

public sealed class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/auth/login", Handler);
    }

    private static async Task<IResult> Handler(ISender sender, LoginRequest req)
    {
        var response = await sender.Send(new LoginCommandRequest(req));
        return Results.Ok(response.Data);
    }
}
