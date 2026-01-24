using Project.Api.Application.Services.Abstractions;
using Project.Api.Application.Services.Implementations;
using System.ComponentModel.DataAnnotations;

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

            group.MapPost("register", async (IAuthService authService, RegisterRequest registerRequest) =>
            {
                try
                {
                    await authService.RegisterAsync(registerRequest);

                    return Results.Created();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
                }
                catch (DbUpdateException ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
                }
            });

            group.MapPatch("change-password", async (IAuthService authService, ChangePasswordRequest passwordRequest, IHttpContextAccessor accessor) =>
            {
                try
                {
                    var userId = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("User id claim not found");

                    await authService.ChangePasswordAsync(Guid.Parse(userId), passwordRequest);

                    return Results.NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
                }
            }).RequireAuthorization();


            group.MapGet("me", async (IAuthService authService, IMemoryCache cache, IHttpContextAccessor accessor) =>
            {
                var userId = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("User id claim not found");


                var response = await cache.GetOrCreateAsync($"currentUser:{userId}", async entry =>
                {
                    entry.AbsoluteExpiration = DateTime.Now.AddHours(6);
                    var user = await authService.GetCurrentUserInfo(Guid.Parse(userId));
                    return user;
                });

                return Results.Ok(response);
            }).RequireAuthorization();

            group.MapDelete("logout", async (IAuthService authService, IHttpContextAccessor accessor) =>
            {
                try
                {
                    await authService.LogoutAsync(accessor.HttpContext?.User!);
                    return Results.NoContent();
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
        }
    }
}