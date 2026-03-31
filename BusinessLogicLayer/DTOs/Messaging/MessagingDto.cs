namespace BusinessLogicLayer.DTOs.Messaging;

public record ConversationSummaryDto(
    long Id,
    long OtherUserId,
    string OtherUserName,
    string? OtherUserAvatar,
    string? LastMessagePreview,
    DateTime? LastMessageAtLocal,
    int UnreadCount
);

public record MessageDto(
    long Id,
    long SenderId,
    bool IsMine,
    string Content,
    bool IsRead,
    DateTime SentAtLocal
);

public record SendMessageDto
{
    public string Content { get; set; } = null!;
}
