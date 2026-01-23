
namespace Project.Api.Presentation.Controllers;

[Route("api/v1/auth"), ApiController]
public sealed class AuthController(IAuthService authService, IValidator<LoginRequest> validator, IMemoryCache cache) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        try
        {
            await authService.RegisterAsync(registerRequest);

            return Created();
        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
        }
        catch (DbUpdateException ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest login, [FromServices] IAuthService authService)
    {
        var validation = validator.Validate(login);
        if (!validation.IsValid) return BadRequest(validation.Errors);

        try
        {
            var token = await authService.LoginAsync(login);
            return Ok(new LoginResponse(token));
        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);

        }

    }


    [HttpPatch("Change-password"), Authorize]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest passwordRequest)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("User id claim not found");

            await authService.ChangePasswordAsync(Guid.Parse(userId), passwordRequest);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("me"), Authorize]
    public async Task<IActionResult> CurrentUserInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("User id claim not found");`


        var response = await cache.GetOrCreateAsync($"currentUser:{userId}", async entry =>
        {
            entry.AbsoluteExpiration = DateTime.Now.AddHours(6);
            var user = await authService.GetCurrentUserInfo(Guid.Parse(userId));
            return user;
        });

        return Ok(response);
    }



    [HttpDelete("logout"), Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await authService.LogoutAsync(User);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

}
