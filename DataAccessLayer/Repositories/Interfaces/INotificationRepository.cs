using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface INotificationRepository
{
    /// <summary>Trả về tối đa <paramref name="limit"/> thông báo gần nhất của user, mới nhất trước.</summary>
    Task<IReadOnlyList<Notification>> GetRecentForUserAsync(long userId, int limit = 10, CancellationToken ct = default);
}
