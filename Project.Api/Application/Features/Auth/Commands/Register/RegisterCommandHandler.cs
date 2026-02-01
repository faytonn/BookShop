namespace Project.Api.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandHandler(IAuthService authService) : IRequestHandler<RegisterCommandRequest, RegisterCommandResponse>
{
    public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest command, CancellationToken cancellationToken)
    {
        await authService.RegisterAsync(command.RegisterRequest);
        return new RegisterCommandResponse();
    }
}

public sealed record RegisterCommandRequest(RegisterRequest RegisterRequest) : IRequest<RegisterCommandResponse>;
public sealed record RegisterCommandResponse();