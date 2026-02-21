namespace Application.CQRS.Auth.Features.Login;

public sealed record LoginCommandRequest(DTOs.LoginRequest LoginRequest) : IRequest<LoginCommandResponse>;
public sealed record LoginCommandResponse(string Token);

public sealed class LoginCommandHandler(IAuthService authService, IValidator<DTOs.LoginRequest> validator) : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
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
