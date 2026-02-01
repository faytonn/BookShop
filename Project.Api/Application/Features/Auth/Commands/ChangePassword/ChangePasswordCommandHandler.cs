namespace Project.Api.Application.Features.Auth.Commands.ChangePassword;

public sealed class ChangePasswordCommandHandler(IAuthService authService) : IRequestHandler<ChangePasswordCommandRequest, ChangePasswordCommandResponse>
{
    public async Task<ChangePasswordCommandResponse> Handle(ChangePasswordCommandRequest command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(command.UserId) || !Guid.TryParse(command.UserId, out var userId))
            throw new InvalidOperationException("User id claim not found.");

        await authService.ChangePasswordAsync(userId, command.ChangePasswordRequest);
        return new ChangePasswordCommandResponse();
    }
}

public sealed record ChangePasswordCommandRequest(string? UserId, ChangePasswordRequest ChangePasswordRequest) : IRequest<ChangePasswordCommandResponse>;
public sealed record ChangePasswordCommandResponse();