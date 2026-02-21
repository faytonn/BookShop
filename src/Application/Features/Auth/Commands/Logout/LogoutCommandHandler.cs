namespace Application.Features.Auth.Commands.Logout;

public sealed class LogoutCommandHandler(IAuthService authService) : IRequestHandler<LogoutCommandRequest, LogoutCommandResponse>
{
    public async Task<LogoutCommandResponse> Handle(LogoutCommandRequest command, CancellationToken cancellationToken)
    {
        await authService.LogoutAsync(command.User);
        return new LogoutCommandResponse();
    }
}

public sealed record LogoutCommandRequest(ClaimsPrincipal User) : IRequest<LogoutCommandResponse>;
public sealed record LogoutCommandResponse();
