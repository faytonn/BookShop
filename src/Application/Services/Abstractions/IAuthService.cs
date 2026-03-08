namespace Application.Services.Abstractions
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest register);
        Task<LoginResponse> LoginAsync(LoginRequest request/*, [FromServices] TokenProvider tokenProvider*/);
        Task ChangePasswordAsync(Guid userId, ChangePasswordRequest passwordRequest);

        Task LogoutAsync(ClaimsPrincipal user);
        Task<UserResponse> GetCurrentUserInfo(Guid userId);

    }
}
