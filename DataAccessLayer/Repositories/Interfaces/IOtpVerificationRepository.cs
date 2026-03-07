using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IOtpVerificationRepository
{
    Task<OtpVerification?> GetLatestActiveByUserIdAsync(long userId);
    Task<OtpVerification> CreateAsync(OtpVerification otp);
    Task UpdateAsync(OtpVerification otp);
    Task InvalidateAllForUserAsync(long userId);
}
