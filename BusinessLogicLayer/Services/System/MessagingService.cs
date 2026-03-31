using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Messaging;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Models;
using DataAccessLayer.Repositories.Interfaces;

namespace BusinessLogicLayer.Services.System;

public class MessagingService(IConversationRepository conversationRepo) : IMessagingService
{
    public async Task<ApiResponse<ConversationSummaryDto>> GetOrCreateConversationAsync(
        long requestingUserId, long otherUserId, CancellationToken ct = default)
    {
        if (requestingUserId == otherUserId)
            return ApiResponse<ConversationSummaryDto>.Fail("Cannot create conversation with yourself.");

        var conv = await conversationRepo.GetOrCreateAsync(requestingUserId, otherUserId, ct);
        var unread = await conversationRepo.CountUnreadAsync(conv.Id, requestingUserId, ct);
        var dto = MapToSummary(conv, requestingUserId, unread);
        return ApiResponse<ConversationSummaryDto>.Ok(dto);
    }

    public async Task<ApiResponse<IReadOnlyList<ConversationSummaryDto>>> GetConversationsAsync(
        long userId, CancellationToken ct = default)
    {
        var convs = await conversationRepo.GetByUserIdAsync(userId, ct);

        var tasks = convs.Select(async c =>
        {
            var unread = await conversationRepo.CountUnreadAsync(c.Id, userId, ct);
            return MapToSummary(c, userId, unread);
        });

        var dtos = (IReadOnlyList<ConversationSummaryDto>)await Task.WhenAll(tasks);
        return ApiResponse<IReadOnlyList<ConversationSummaryDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<IReadOnlyList<MessageDto>>> GetMessagesAsync(
        long conversationId, long requestingUserId,
        int skip = 0, int take = 50,
        CancellationToken ct = default)
    {
        var conv = await conversationRepo.GetWithMessagesAsync(conversationId, requestingUserId, skip, take, ct);
        if (conv is null)
            return ApiResponse<IReadOnlyList<MessageDto>>.Fail("Conversation not found.");

        var dtos = (IReadOnlyList<MessageDto>)conv.Messages
            .Select(m => MapToMessageDto(m, requestingUserId))
            .ToList();

        return ApiResponse<IReadOnlyList<MessageDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<MessageDto>> SendMessageAsync(
        long conversationId, long senderId, SendMessageDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Content))
            return ApiResponse<MessageDto>.Fail("Message content cannot be empty.");

        // Verify sender is a participant
        var conv = await conversationRepo.GetWithMessagesAsync(conversationId, senderId, 0, 0, ct);
        if (conv is null)
            return ApiResponse<MessageDto>.Fail("Conversation not found.");

        var message = new Message
        {
            ConversationId = conversationId,
            SenderId       = senderId,
            Content        = dto.Content.Trim(),
        };

        var saved = await conversationRepo.AddMessageAsync(message, ct);
        return ApiResponse<MessageDto>.Ok(MapToMessageDto(saved, senderId));
    }

    public async Task<ApiResponse<bool>> MarkReadAsync(
        long conversationId, long userId, CancellationToken ct = default)
    {
        await conversationRepo.MarkReadAsync(conversationId, userId, ct);
        return ApiResponse<bool>.Ok(true);
    }

    // ── Mappers ──────────────────────────────────────────────────────────────

    private static ConversationSummaryDto MapToSummary(Conversation conv, long requestingUserId, int unread)
    {
        var isA = conv.ParticipantAId == requestingUserId;
        var other = isA ? conv.ParticipantB : conv.ParticipantA;

        var lastMsg = conv.Messages.MaxBy(m => m.CreatedAt);
        var preview = lastMsg?.Content is { } c
            ? (c.Length > 60 ? c[..57] + "..." : c)
            : null;

        var otherName = other.TeacherProfile?.FullName
                     ?? other.StudentProfile?.FullName
                     ?? other.Email;

        return new ConversationSummaryDto(
            Id:                   conv.Id,
            OtherUserId:          other.Id,
            OtherUserName:        otherName,
            OtherUserAvatar:      other.AvatarUrl,
            LastMessagePreview:   preview,
            LastMessageAtLocal:   conv.LastMessageAt?.ToLocalTime(),
            UnreadCount:          unread
        );
    }

    private static MessageDto MapToMessageDto(Message m, long requestingUserId) => new(
        Id:         m.Id,
        SenderId:   m.SenderId,
        IsMine:     m.SenderId == requestingUserId,
        Content:    m.Content,
        IsRead:     m.IsRead,
        SentAtLocal: m.CreatedAt.ToLocalTime()
    );
}
