using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface ITeacherCourseRepository
{
    Task<Class> CreateAsync(Class cls);
    Task<Class?> GetTeacherClassDetailAsync(long classId, long teacherId, CancellationToken ct = default);
    Task<IReadOnlyList<Class>> GetByTeacherIdAsync(long teacherId, CancellationToken ct = default);
    Task<Material> AddMaterialAsync(Material material, CancellationToken ct = default);
    Task DeleteMaterialAsync(long materialId, long classId, CancellationToken ct = default);

    Task<Assignment?> GetAssignmentDetailAsync(long assignmentId, long classId, long teacherId, CancellationToken ct = default);
    Task<Submission?> GradeSubmissionAsync(long submissionId, long teacherId, decimal score, string? feedback, CancellationToken ct = default);
    Task<Class> UpdateAsync(Class cls);

    /// <summary>Lấy tất cả lớp ACTIVE kèm teacher profile + schedules + enrolled count.</summary>
    Task<IReadOnlyList<Class>> GetActiveClassListingsAsync(
        string? subject = null,
        decimal? maxRate = null,
        CancellationToken ct = default);
}
