namespace Application.CQRS.Auth.Features.Register;

public sealed record RegisterCommandRequest(DTOs.RegisterRequest RegisterRequest) : IRequest<RegisterCommandResponse>;
public sealed record RegisterCommandResponse();

public sealed class RegisterCommandHandler(IAuthService authService) : IRequestHandler<RegisterCommandRequest, RegisterCommandResponse>
{
    public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest command, CancellationToken cancellationToken)
    {
        await authService.RegisterAsync(command.RegisterRequest);
        return new RegisterCommandResponse();
    }
}
