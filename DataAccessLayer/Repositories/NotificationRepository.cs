using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class NotificationRepository(AppDbContext db) : INotificationRepository
{
    public async Task<IReadOnlyList<Notification>> GetRecentForUserAsync(
        long userId, int limit = 10, CancellationToken ct = default) =>
        await db.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(limit)
            .ToListAsync(ct);
}
