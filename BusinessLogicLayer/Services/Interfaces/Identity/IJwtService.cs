namespace BusinessLogicLayer.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(long userId, string email, string role);
    bool IsTokenBlacklisted(string token);
    void BlacklistToken(string token, DateTime expiry);
}
