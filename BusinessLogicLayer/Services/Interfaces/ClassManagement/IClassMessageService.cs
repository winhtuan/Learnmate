using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Messaging;

namespace BusinessLogicLayer.Services.Interfaces.ClassManagement;

public interface IClassMessageService
{
    Task<ApiResponse<IReadOnlyList<ClassMessageDto>>> GetClassMessagesAsync(long classId, long userId, int skip, int take, CancellationToken ct);
    Task<ApiResponse<ClassMessageDto>> SendClassMessageAsync(long classId, long userId, SendClassMessageDto dto, CancellationToken ct);
}
