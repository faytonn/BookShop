using Project.Api.Domain.Entities;

namespace Project.Api.Application.DTOs;

public record struct RegisterDto(string Email, string Password, UserRole UserRole = UserRole.None);

public record struct LoginDto(string Email, string Password);

