namespace Application.CQRS.Auth.Features.GetCurrentUser;

public sealed class GetCurrentUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/auth/me", Handler)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handler(ISender sender, IHttpContextAccessor accessor)
    {
        var userId = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await sender.Send(new GetCurrentUserQueryRequest(userId));
        return Results.Ok(response);
    }
}
