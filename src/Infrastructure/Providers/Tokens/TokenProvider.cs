using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Infrastructure.Providers.Tokens;

public sealed class TokenProvider(IConfiguration cfg)
{
    public (string, DateTime) GenerateJwtToken(Domain.Models.User user)
    {
        var claims = new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, Enum.GetName(user.Role)!)
        ]);

        var securityKey = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg.GetValue<string>("Jwt:SecurityKey")!)),
            SecurityAlgorithms.HmacSha256
        );

        var expirationTime = DateTime.UtcNow.AddMinutes(cfg.GetValue<int>("Jwt:ExpirationMinutes"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            SigningCredentials = securityKey,
            Expires = expirationTime,
            Issuer = "",
            Audience = ""
        };

        return (new JsonWebTokenHandler().CreateToken(tokenDescriptor), expirationTime);
    }

    public (string, DateTime) GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var expirationTime = DateTime.UtcNow.AddDays(cfg.GetValue<int>("Jwt:RefreshTokenExpirationDays"));
        return (Convert.ToBase64String(randomBytes), expirationTime);
    }
}
