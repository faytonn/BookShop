namespace Project.Api.Application.DTOs;

public record struct RegisterRequest(string Email, string Password, UserRole UserRole = UserRole.None);
//public record struct RegisterResponse(string Email, string Password, UserRole UserRole = UserRole.None);

public record struct LoginRequest(string Email, string Password);
public record struct LoginResponse(string Token);

public record struct ChangePasswordRequest(string OldPassword, string NewPassword);


    