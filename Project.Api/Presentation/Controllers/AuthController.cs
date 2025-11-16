using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Api.Application.DTOs;
using Project.Api.Domain.Entities;
using Project.Api.Infrastucture.Providers.Tokens;
using Project.Api.Persistence.Contexts;

namespace Project.Api.Presentation.Controllers;

[Route("api/v1/auth"), ApiController]
public sealed class AuthController(AppDbContext context) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto register)
    {
        if (string.IsNullOrEmpty(register.Email.Trim()) || string.IsNullOrEmpty(register.Password.Trim())) return BadRequest("Invalid format for email or password");

        var isExist = await context.Users.Where(e => e.Email.Equals(register.Email.ToLower())).AnyAsync();

        if (isExist) return BadRequest("User already exists!");

        var newUser = new User
        {
            Email = register.Email.ToLower(),
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(register.Password, BCrypt.Net.BCrypt.GenerateSalt()),
            Role = register.UserRole
        };

        try
        {
            context.Users.Add(newUser);
            await context.SaveChangesAsync();

            return Created();
        }
        catch (DbUpdateException ex)
        {
            return BadRequest($"An error occured: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"An error occured: {ex.Message}");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto login, [FromServices] TokenProvider tokenProvider)
    {
        if (string.IsNullOrEmpty(login.Email.Trim()) || string.IsNullOrEmpty(login.Password.Trim())) return BadRequest("Invalid format for email or password");
        var user = await context.Users.Where(e => e.Email.Equals(login.Email.ToLower())).FirstOrDefaultAsync();
        if (user is null || !BCrypt.Net.BCrypt.Verify(login.Password, user.HashedPassword)) return BadRequest("Invalid email or password!");

        var token = tokenProvider.GenerateJwtToken(user);

        return Ok(new { Token = token });
    }
}
