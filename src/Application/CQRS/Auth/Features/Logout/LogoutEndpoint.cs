namespace Application.CQRS.Auth.Features.Logout;

public sealed class LogoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/auth/logout", Handler);
    }

    private static async Task<IResult> Handler(ISender sender, IHttpContextAccessor accessor)
    {
        await sender.Send(new LogoutCommandRequest(accessor.HttpContext?.User!));
        return Results.NoContent();
    }
}
