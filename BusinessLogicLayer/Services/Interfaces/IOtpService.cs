namespace BusinessLogicLayer.Services.Interfaces;

public interface IOtpService
{
    string GenerateCode();
    DateTime GetExpiryTime();
    bool IsExpired(DateTime expiredAt);
}
