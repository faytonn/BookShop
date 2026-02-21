namespace Application.CQRS.Auth.Features.ChangePassword;

public sealed class ChangePasswordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/v1/auth/change-password", Handler)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handler(ISender sender, DTOs.ChangePasswordRequest req, IHttpContextAccessor accessor)
    {
        var userId = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        await sender.Send(new ChangePasswordCommandRequest(userId, req));
        return Results.NoContent();
    }
}
