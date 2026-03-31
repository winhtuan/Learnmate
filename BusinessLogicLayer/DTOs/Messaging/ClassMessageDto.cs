namespace BusinessLogicLayer.DTOs.Messaging;

public record ClassMessageDto(
    long Id,
    long ClassId,
    long SenderId,
    string SenderName,
    string? SenderAvatar,
    string Content,
    DateTime SentAt,
    DateTime SentAtLocal,
    bool IsMine
);
