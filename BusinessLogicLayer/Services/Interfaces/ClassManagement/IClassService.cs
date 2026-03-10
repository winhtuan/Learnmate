using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Class;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IClassService
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
}
