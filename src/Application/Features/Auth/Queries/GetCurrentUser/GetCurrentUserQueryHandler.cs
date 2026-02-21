namespace Application.Features.Auth.Queries.GetCurrentUser;

public sealed class GetCurrentUserQueryHandler(IAuthService authService, IMemoryCache cache) : IRequestHandler<GetCurrentUserQueryRequest, GetCurrentUserQueryResponse>
{
    public async Task<GetCurrentUserQueryResponse> Handle(GetCurrentUserQueryRequest query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(query.UserId) || !Guid.TryParse(query.UserId, out var userId))
            throw new InvalidOperationException("User id claim not found.");

        var user = await cache.GetOrCreateAsync($"currentUser:{userId}", async entry =>
        {
            entry.AbsoluteExpiration = DateTime.Now.AddHours(6);
            return await authService.GetCurrentUserInfo(userId);
        });

        return new GetCurrentUserQueryResponse(user);
    }
}

public sealed record GetCurrentUserQueryRequest(string? UserId) : IRequest<GetCurrentUserQueryResponse>;
public sealed record GetCurrentUserQueryResponse(UserResponse? User);
