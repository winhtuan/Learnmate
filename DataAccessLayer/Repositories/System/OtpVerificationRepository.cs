using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class OtpVerificationRepository(AppDbContext db) : IOtpVerificationRepository
{
    public Task<OtpVerification?> GetLatestActiveByUserIdAsync(long userId) =>
        db
            .OtpVerifications.Where(o => o.UserId == userId && !o.IsUsed)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

    public async Task<OtpVerification> CreateAsync(OtpVerification otp)
    {
        db.OtpVerifications.Add(otp);
        await db.SaveChangesAsync();
        return otp;
    }

    public async Task UpdateAsync(OtpVerification otp)
    {
        db.OtpVerifications.Update(otp);
        await db.SaveChangesAsync();
    }

    public async Task InvalidateAllForUserAsync(long userId)
    {
        var otps = await db
            .OtpVerifications.Where(o => o.UserId == userId && !o.IsUsed)
            .ToListAsync();

        foreach (var otp in otps)
            otp.IsUsed = true;

        await db.SaveChangesAsync();
    }
}
