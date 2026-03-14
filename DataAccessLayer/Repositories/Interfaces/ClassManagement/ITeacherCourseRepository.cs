using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface ITeacherCourseRepository
{
    Task<Class> CreateAsync(Class cls);
    Task<Class?> GetTeacherClassDetailAsync(long classId, long teacherId, CancellationToken ct = default);
    Task<IReadOnlyList<Class>> GetByTeacherIdAsync(long teacherId, CancellationToken ct = default);
}
