namespace Application.CQRS.Auth.Features.RefreshToken;

public sealed record RefreshTokenCommandRequest(RefreshTokenEndpoint.RefreshTokenDto Dto) : IRequest<RefreshTokenCommandResponse>;
public sealed record RefreshTokenCommandResponse(LoginResponse Data);

public sealed class RefreshTokenCommandHandler(IUnitOfWork unitOfWork, TokenProvider tokenProvider)
    : IRequestHandler<RefreshTokenCommandRequest, RefreshTokenCommandResponse>
{
    public async Task<RefreshTokenCommandResponse> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var principal = tokenProvider.GetPrincipalFromExpiredToken(dto.AccessToken);
        var userEmail = principal.FindFirstValue(ClaimTypes.Email) ?? throw new BadRequestException("No user email claim!");

        var user = await unitOfWork.Users.GetWhereAll(e => e.Email == userEmail).FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("User not found!");

        if (!user.RefreshToken.SequenceEqual(Convert.FromBase64String(dto.RefreshToken))
            || user.RefreshTokenExpirationTime < DateTime.UtcNow) throw new BadRequestException("Invalid refresh token!");

        var (accessToken, accessTokenExpiration) = tokenProvider.GenerateJwtToken(user);
        var (refreshToken, refreshTokenExpiration) = tokenProvider.GenerateRefreshToken();

        return new RefreshTokenCommandResponse(new LoginResponse(
            accessToken,
            refreshToken,
            accessTokenExpiration,
            refreshTokenExpiration
        ));
    }
}