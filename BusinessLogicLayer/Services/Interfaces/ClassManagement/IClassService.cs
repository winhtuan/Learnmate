using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Class;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IStudentClassService
{
    Task<ApiResponse<IReadOnlyList<ClassListItemDto>>> GetEnrolledClassesAsync(
        long studentId,
        CancellationToken ct = default
    );

    Task<ApiResponse<ClassDetailDto>> GetClassDetailAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    );

    Task<ApiResponse<IReadOnlyList<ClassAssignmentDto>>> GetClassAssignmentsAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    );

    Task<ApiResponse<IReadOnlyList<ClassScheduleItemDto>>> GetClassSchedulesAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    );

    Task<ApiResponse<IReadOnlyList<ClassMaterialDto>>> GetClassMaterialsAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    );

    Task<ApiResponse<bool>> LeaveClassAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    );

    Task<ApiResponse<bool>> SubmitAssignmentAsync(
        long classId,
        long assignmentId,
        long studentId,
        Stream? fileStream,
        string? fileName,
        string? contentType,
        CancellationToken ct = default
    );

    Task<ApiResponse<ClassMaterialDto>> UploadMaterialAsync(
        long classId,
        long userId,
        string title,
        Stream fileStream,
        string fileName,
        string contentType,
        long? fileSizeBytes = null,
        CancellationToken ct = default
    );

    Task<ApiResponse<ClassVideosDto>> GetClassVideosAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    );
}
