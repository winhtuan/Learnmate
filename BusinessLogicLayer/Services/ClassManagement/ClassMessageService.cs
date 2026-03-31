using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Messaging;
using BusinessLogicLayer.Services.Interfaces.ClassManagement;
using BusinessObject.Models;
using DataAccessLayer.Repositories.Interfaces.ClassManagement;

namespace BusinessLogicLayer.Services.ClassManagement;

public class ClassMessageService(
    IClassMessageRepository classMessageRepo) : IClassMessageService
{
    public async Task<ApiResponse<IReadOnlyList<ClassMessageDto>>> GetClassMessagesAsync(
        long classId, long userId, int skip, int take, CancellationToken ct)
    {
        var messages = await classMessageRepo.GetClassMessagesAsync(classId, skip, take, ct);

        // Map to Dto
        var dtos = messages.Select(m => new ClassMessageDto(
            m.Id,
            m.ClassId,
            m.SenderId,
            m.Sender?.TeacherProfile?.FullName ?? m.Sender?.StudentProfile?.FullName ?? m.Sender?.Email ?? "Unknown",
            m.Sender?.AvatarUrl,
            m.Content,
            m.CreatedAt,
            m.CreatedAt.ToLocalTime(),
            m.SenderId == userId
        )).OrderBy(m => m.SentAt).ToList();

        return ApiResponse<IReadOnlyList<ClassMessageDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<ClassMessageDto>> SendClassMessageAsync(
        long classId, long userId, SendClassMessageDto dto, CancellationToken ct)
    {
        var msg = new ClassMessage
        {
            ClassId = classId,
            SenderId = userId,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow
        };

        var saved = await classMessageRepo.AddAsync(msg, ct);
        if (!saved) return ApiResponse<ClassMessageDto>.Fail("Failed to send message");

        var recent = await classMessageRepo.GetClassMessagesAsync(classId, 0, 1, ct);
        var m = recent.FirstOrDefault();

        if (m == null) return ApiResponse<ClassMessageDto>.Fail("Failed to retrieve sent message");

        var resultDto = new ClassMessageDto(
            m.Id,
            m.ClassId,
            m.SenderId,
            m.Sender?.TeacherProfile?.FullName ?? m.Sender?.StudentProfile?.FullName ?? m.Sender?.Email ?? "Unknown",
            m.Sender?.AvatarUrl,
            m.Content,
            m.CreatedAt,
            m.CreatedAt.ToLocalTime(),
            true
        );

        return ApiResponse<ClassMessageDto>.Ok(resultDto);
    }
}
