namespace Application.CQRS.Auth.Features.RefreshToken;

public sealed class RefreshTokenEndpoint : ICarterModule
{
    public sealed record RefreshTokenDto(string AccessToken, string RefreshToken);

    public sealed class RefreshTokenValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.AccessToken).NotNull().NotEmpty().WithMessage("Access token is required.");
            RuleFor(x => x.RefreshToken).NotNull().NotEmpty().WithMessage("Refresh token is required.");
        }
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/auth/refresh-token", Handler);
    }

    private static async Task<IResult> Handler(RefreshTokenDto dto, IValidator<RefreshTokenDto> validator, ISender sender)
    {
        var validationResult = await validator.ValidateAsync(dto);
        
        if (!validationResult.IsValid) return Results.BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var response = await sender.Send(new RefreshTokenCommandRequest(dto));
        return Results.Ok(response.Data);
    }
}