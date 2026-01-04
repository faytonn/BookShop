namespace Project.Api.Application.Services.Abstractions
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest register);
        Task<string> LoginAsync(LoginRequest request/*, [FromServices] TokenProvider tokenProvider*/);
        Task ChangePasswordAsync(Guid userId, ChangePasswordRequest passwordRequest);

        Task LogoutAsync(ClaimsPrincipal user);

    }
}
