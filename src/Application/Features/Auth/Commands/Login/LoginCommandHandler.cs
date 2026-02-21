namespace Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(IAuthService authService, IValidator<LoginRequest> validator) : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
{
    public async Task<LoginCommandResponse> Handle(LoginCommandRequest command, CancellationToken cancellationToken)
    {
        var validation = validator.Validate(command.LoginRequest);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var token = await authService.LoginAsync(command.LoginRequest);
        return new LoginCommandResponse(token);
    }
}

public sealed record LoginCommandRequest(LoginRequest LoginRequest) : IRequest<LoginCommandResponse>;
public sealed record LoginCommandResponse(string Token);
