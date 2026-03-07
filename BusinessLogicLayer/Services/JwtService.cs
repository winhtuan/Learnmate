using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogicLayer.Services;

public class JwtService(IConfiguration config, IMemoryCache cache) : IJwtService
{
    private const string BlacklistPrefix = "blacklist_";

    private string SecretKey =>
        config["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey is not configured.");

    private string Issuer => config["Jwt:Issuer"] ?? "learnmate";
    private string Audience => config["Jwt:Audience"] ?? "learnmate";

    private int ExpiryMinutes =>
        config.GetValue<int>("Jwt:ExpiryMinutes", 60);

    public string GenerateToken(long userId, string email, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

        var expiry = DateTime.UtcNow.AddMinutes(ExpiryMinutes);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: expiry,
            signingCredentials: credentials);

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
