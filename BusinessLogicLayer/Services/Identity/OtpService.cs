using BusinessLogicLayer.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BusinessLogicLayer.Services;

public class OtpService(IConfiguration config) : IOtpService
{
    private static readonly Random _rng = Random.Shared;

    private int ExpiryMinutes => config.GetValue<int>("OtpSettings:ExpiryMinutes", 5);

    public string GenerateCode() => _rng.Next(100_000, 1_000_000).ToString();

    public DateTime GetExpiryTime() => DateTime.UtcNow.AddMinutes(ExpiryMinutes);

    public bool IsExpired(DateTime expiredAt) => DateTime.UtcNow > expiredAt;
}
