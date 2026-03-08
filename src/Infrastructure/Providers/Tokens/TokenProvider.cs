using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Providers.Tokens;

public sealed class TokenProvider(IConfiguration cfg)
{
    public (string, DateTime) GenerateJwtToken(User user)
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
            Issuer = cfg.GetValue<string>("Jwt:Issuer"),
            Audience = cfg.GetValue<string>("Jwt:Audience")
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

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg.GetValue<string>("Jwt:SecurityKey")!)),
                    ValidAudience = cfg.GetValue<string>("Jwt:Audience")!,
                    ValidIssuer = cfg.GetValue<string>("Jwt:Issuer")!,
                },
                out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Token is invalid.");
        }
        return principal;
    }
}
