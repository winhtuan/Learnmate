using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IConversationRepository
{
    /// <summary>Tìm hoặc tạo conversation giữa 2 user. Normalize: A = min(id), B = max(id).</summary>
    Task<Conversation> GetOrCreateAsync(long userAId, long userBId, CancellationToken ct = default);

    /// <summary>Danh sách conversations của user, kèm participant info + last message preview.</summary>
    Task<IReadOnlyList<Conversation>> GetByUserIdAsync(long userId, CancellationToken ct = default);

    /// <summary>Lấy conversation + messages phân trang. Trả null nếu user không phải participant.</summary>
    Task<Conversation?> GetWithMessagesAsync(
        long conversationId, long requestingUserId,
        int skip = 0, int take = 50,
        CancellationToken ct = default);

    /// <summary>Lưu message mới và cập nhật LastMessageAt trên conversation.</summary>
    Task<Message> AddMessageAsync(Message message, CancellationToken ct = default);

    /// <summary>Đánh dấu messages chưa đọc trong conversation là đã đọc (chỉ messages của người kia).</summary>
    Task MarkReadAsync(long conversationId, long readByUserId, CancellationToken ct = default);

    /// <summary>Đếm số messages chưa đọc trong conversation của user.</summary>
    Task<int> CountUnreadAsync(long conversationId, long userId, CancellationToken ct = default);
}
