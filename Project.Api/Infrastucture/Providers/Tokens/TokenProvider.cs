using Microsoft.IdentityModel.JsonWebTokens;

namespace Project.Api.Infrastucture.Providers.Tokens;

public sealed class TokenProvider(IConfiguration cfg)
{
    public string GenerateJwtToken(User user)
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
