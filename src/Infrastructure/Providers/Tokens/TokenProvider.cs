using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Providers.Tokens;

public sealed class TokenProvider(IConfiguration cfg)
{
    public string GenerateJwtToken(Domain.Models.User user)
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

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            SigningCredentials = securityKey,
            Expires = DateTime.UtcNow.AddMinutes(cfg.GetValue<int>("Jwt:ExpirationMinutes")),
            Issuer = "",
            Audience = ""
        };

        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }
}
