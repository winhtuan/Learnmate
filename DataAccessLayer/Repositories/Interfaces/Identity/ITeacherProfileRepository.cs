using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface ITeacherProfileRepository
{
    Task<TeacherProfile?> GetByUserIdAsync(long userId);
    Task UpdateAsync(TeacherProfile profile);

    /// <summary>
    /// Lấy tất cả giáo viên có TeacherProfile với optional filter.
    /// subjectFilter: tìm trong Subjects (comma-separated).
    /// maxRate: lọc HourlyRate &lt;= maxRate.
    /// </summary>
    Task<IReadOnlyList<TeacherProfile>> GetAllTeachersAsync(
        string? subjectFilter = null,
        decimal? maxRate = null,
        CancellationToken ct = default);
}

