using Azure.Core;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Project.Api.Application.DTOs;
using Project.Api.Application.Services.Abstractions;
using Project.Api.Domain.Entities;
using Project.Api.Infrastucture.Providers.Tokens;
using Project.Api.Persistence.Contexts;
using Project.Api.Persistence.Repositories.Users;
using Project.Api.Persistence.UnitOfWorks;
using System.Net;
using System.Security.Claims;

namespace Project.Api.Application.Services.Implementations;

public sealed class AuthService(IUnitOfWork unitOfWork, /*[FromServices] */TokenProvider tokenProvider) : IAuthService
{
    public async Task<string> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Email.Trim()) || string.IsNullOrEmpty(request.Password.Trim())) 
            throw new /*BadRequest*/InvalidOperationException("Invalid format for email or password");
        var user = await unitOfWork.Users.GetWhereAll(e => e.Email.Equals(request.Email.ToLower()))
                                       .FirstOrDefaultAsync();
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword)) throw new /*BadRequest*/InvalidOperationException("Invalid format for email or password");

        var token = tokenProvider.GenerateJwtToken(user);

        return token;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        if (string.IsNullOrEmpty(request.Email.Trim()) || string.IsNullOrEmpty(request.Password.Trim())) throw new InvalidOperationException("Invalid format for email or password");
        var requestEmail = request.Email.Trim().ToLower();
        var isExist = await unitOfWork.Users.GetWhereAll(e => e.Email.Equals(requestEmail))
                                          .AnyAsync();

        if (isExist) throw new/* BadRequest*/InvalidOperationException("User already exists!");

        var newUser = new User
        {
            Email = requestEmail,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, BCrypt.Net.BCrypt.GenerateSalt()),
            Role = request.UserRole
        };

        try
        {
            unitOfWork.Users.Add(newUser);
            await unitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new DbUpdateException($"An error occured: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occured: {ex.Message}");
        }
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequest passwordRequest)
    {
        var user = await unitOfWork.Users.FindAsync(userId);
        if (user is null)
            throw new InvalidOperationException("No such user exists.");

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(passwordRequest.OldPassword, user.HashedPassword);

        if (!isPasswordValid)
            throw new InvalidOperationException("Passwords do not match.");

        if (passwordRequest.OldPassword == passwordRequest.NewPassword)
            throw new InvalidOperationException("New password cannot be the same with the old password.");

        try
        {
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordRequest.NewPassword, BCrypt.Net.BCrypt.GenerateSalt());
            await unitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new DbUpdateException($"An error occured: {ex.Message}");
        }
        catch (BCrypt.Net.SaltParseException ex)
        {
            throw new BCrypt.Net.SaltParseException($"An error occured: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"An error occured: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occured: {ex.Message}");
        }
    }

    public Task LogoutAsync(ClaimsPrincipal user)
    {
        var identity = user.Identity as ClaimsIdentity;

        if (identity is not null)
        {
            foreach (var claim in identity.Claims.ToList())
                identity.RemoveClaim(claim);

            return Task.CompletedTask;
        }

        throw new InvalidOperationException("Identity not found!");
    }
}
