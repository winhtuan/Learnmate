using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLogicLayer.Services.Interfaces;
using BusinessLogicLayer.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogicLayer.Services;

public class JwtService(IOptions<JwtSettings> options, IMemoryCache cache) : IJwtService
{
    private const string BlacklistPrefix = "blacklist_";
    private readonly JwtSettings _settings = options.Value;

    public string GenerateToken(long userId, string email, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(
                JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64
            ),
        };

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool IsTokenBlacklisted(string token) =>
        cache.TryGetValue(BlacklistPrefix + token, out _);

    public void BlacklistToken(string token, DateTime expiry)
    {
        var ttl = expiry - DateTime.UtcNow;
        if (ttl > TimeSpan.Zero)
            cache.Set(BlacklistPrefix + token, true, ttl);
    }
}
