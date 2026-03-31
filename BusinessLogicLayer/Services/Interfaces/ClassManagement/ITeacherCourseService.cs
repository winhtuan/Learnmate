using BusinessLogicLayer.DTOs.Teacher.Classes;

namespace BusinessLogicLayer.Services.Interfaces;

public interface ITeacherCourseService
{
    Task<long> CreateClassAsync(long teacherId, CreateClassDto dto);

    Task<TeacherAssignmentFullDetailDto?> GetAssignmentDetailAsync(long assignmentId, long classId, long teacherId, CancellationToken ct = default);
    Task GradeSubmissionAsync(long submissionId, long teacherId, decimal score, string? feedback, CancellationToken ct = default);

    Task<TeacherClassDetailDto?> GetTeacherClassDetailAsync(long classId, long teacherId, CancellationToken ct = default);
    Task<List<TeacherClassListItemDto>> GetTeacherClassesAsync(long teacherId, CancellationToken ct = default);

    /// <summary>
    /// Upload a file (PDF/Word/etc.) to cloud storage and save material metadata to DB.
    /// Returns the new material DTO.
    /// </summary>
    Task<TeacherMaterialItemDto> UploadMaterialAsync(
        long classId, long teacherId,
        string fileName, string title, string? description,
        Stream fileContent, string contentType, long fileSizeBytes,
        CancellationToken ct = default);

    /// <summary>
    /// Soft-delete a material (set status = HIDDEN) and remove file from cloud.
    /// </summary>
    Task DeleteMaterialAsync(long classId, long teacherId, long materialId, CancellationToken ct = default);
}
