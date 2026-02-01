namespace Project.Api.Presentation.Endpoints;

public static class AuthEndpoints
{
    extension(IEndpointRouteBuilder route)
    {
        public void MapAuthEndpoints()
        {
            var group = route.MapGroup("api/v1/auth");

            group.MapPost("login", async (ISender sender, LoginRequest req) =>
            {
                var response = await sender.Send(new LoginCommandRequest(req));

                return Results.Ok(new LoginResponse(response.Token));
            });

            group.MapPost("register", async (ISender sender, RegisterRequest req) =>
            {
                await sender.Send(new RegisterCommandRequest(req));

                return Results.Created();
            });

            group.MapPatch("change-password", async (ISender sender, ChangePasswordRequest req, IHttpContextAccessor accessor) =>
            {
                var userId = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                await sender.Send(new ChangePasswordCommandRequest(userId, req));

                return Results.NoContent();
            })
            .RequireAuthorization();

            group.MapGet("me", async (ISender sender, IHttpContextAccessor accessor) =>
            {
                var userId = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var response = await sender.Send(new GetCurrentUserQueryRequest(userId));

                return Results.Ok(response);
            })
             .RequireAuthorization();

            group.MapDelete("logout", async (ISender sender, IHttpContextAccessor accessor) =>
            {
                await sender.Send(new LogoutCommandRequest(accessor.HttpContext?.User!));

                return Results.NoContent();
            });
        }
    }
}