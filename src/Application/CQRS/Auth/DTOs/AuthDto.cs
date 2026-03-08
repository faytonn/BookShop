namespace Application.CQRS.Auth.DTOs;

public record struct RegisterRequest(string Name, string Surname, string Email, string Password, UserRole UserRole = UserRole.None);

public record struct LoginRequest(string Email, string Password);
public record struct LoginResponse(string AccessToken, string RefreshToken, DateTime AccessTokenExpiration, DateTime RefreshTokenExpiration);

public record struct UserResponse(Guid Id, string? Name, string? Surname, string Email, string UserRole, DateTime? LastLoggedAt);

public record struct ChangePasswordRequest(string OldPassword, string NewPassword);
