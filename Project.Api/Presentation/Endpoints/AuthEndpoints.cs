namespace Project.Api.Presentation.Endpoints;

public static class AuthEndpoints
{
    extension(IEndpointRouteBuilder route)
    {
        public void MapAuthEndpoints()
        {
            var group = route.MapGroup("auth");

            group.MapPost("login", async (LoginRequest login, IValidator<LoginRequest> validator, [FromServices] IAuthService authService) =>
            {
                var validation = validator.Validate(login);
                if (!validation.IsValid) return Results.BadRequest(validation.Errors);

                try
                {
                    var token = await authService.LoginAsync(login);
                    return Results.Ok(new LoginResponse(token));
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
                }
            });

            group.MapPost("register", () =>
            {

            });
        }
    }
}