using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LearnmateSolution.Services;

/// <summary>
/// Chạy nền mỗi 5 phút, tự động EXPIRED các booking AWAITING_PAYMENT đã quá hạn
/// </summary>
public class BookingExpiryBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<BookingExpiryBackgroundService> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("BookingExpiryService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckAndExpireBookingsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "BookingExpiryService error during cycle.");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    private async Task CheckAndExpireBookingsAsync(CancellationToken ct)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var bookingRepo = scope.ServiceProvider.GetRequiredService<ITutorBookingRepository>();
        var db          = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var expired = await bookingRepo.GetExpiredAwaitingPaymentAsync(ct);

        if (!expired.Any()) return;

        logger.LogInformation("BookingExpiryService: Found {Count} expired bookings to process.", expired.Count);

        foreach (var booking in expired)
        {
            booking.Status       = BookingRequestStatus.EXPIRED;
            booking.CancelReason = "Hết thời hạn thanh toán. Booking đã tự động bị huỷ.";
            booking.UpdatedAt    = DateTime.UtcNow;
            await bookingRepo.UpdateAsync(booking, ct);

            // Gửi thông báo cho student
            try
            {
                db.Notifications.Add(new Notification
                {
                    UserId    = booking.StudentId,
                    Title     = "Booking đã hết hạn thanh toán",
                    Content   = $"Booking của bạn với giáo viên đã hết thời hạn thanh toán và bị huỷ tự động. Bạn có thể đặt lịch mới bất kỳ lúc nào.",
                    IsRead    = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                });
                await db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send expiry notification for booking {Id}", booking.Id);
            }
        }
    }
}
