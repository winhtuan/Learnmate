using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Messaging;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IMessagingService
{
    Task<ApiResponse<ConversationSummaryDto>> GetOrCreateConversationAsync(
        long requestingUserId, long otherUserId, CancellationToken ct = default);

    Task<ApiResponse<IReadOnlyList<ConversationSummaryDto>>> GetConversationsAsync(
        long userId, CancellationToken ct = default);

    Task<ApiResponse<IReadOnlyList<MessageDto>>> GetMessagesAsync(
        long conversationId, long requestingUserId,
        int skip = 0, int take = 50,
        CancellationToken ct = default);

    Task<ApiResponse<MessageDto>> SendMessageAsync(
        long conversationId, long senderId, SendMessageDto dto, CancellationToken ct = default);

    Task<ApiResponse<bool>> MarkReadAsync(
        long conversationId, long userId, CancellationToken ct = default);
}
