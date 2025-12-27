using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Api.Application.DTOs;
using Project.Api.Application.Services.Abstractions;
using System.Net;
using System.Security.Claims;

namespace Project.Api.Presentation.Controllers;

[Route("api/v1/auth"), ApiController]
public sealed class AuthController(IAuthService authService) : ControllerBase
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
        catch(Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
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
