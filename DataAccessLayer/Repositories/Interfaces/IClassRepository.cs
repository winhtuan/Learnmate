using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IClassRepository
{
    /// <summary>Creates a new class and returns it with its generated Id.</summary>
    Task<Class> CreateAsync(Class cls);

    /// <summary>Returns full class detail for teacher view (students, assignments, schedules, materials).</summary>
    Task<Class?> GetTeacherClassDetailAsync(long classId, long teacherId, CancellationToken ct = default);

    /// <summary>Returns all classes owned by a teacher, with member count and next schedule.</summary>
    Task<IReadOnlyList<Class>> GetByTeacherIdAsync(long teacherId, CancellationToken ct = default);

    /// <summary>Returns all ACTIVE classes the student is enrolled in, with teacher profile and next schedule.</summary>
    Task<IReadOnlyList<Class>> GetEnrolledWithDetailsAsync(long studentId, CancellationToken ct = default);

    /// <summary>Returns a single class with full details: teacher, assignments (with student submissions), schedules, materials.</summary>
    Task<Class?> GetByIdWithDetailsAsync(long classId, long studentId, CancellationToken ct = default);

    /// <summary>Returns assignments for a class with the student's submission.</summary>
    Task<IReadOnlyList<(Assignment Assignment, Submission? Submission)>> GetAssignmentsWithSubmissionsAsync(
        long classId, long studentId, CancellationToken ct = default);

    /// <summary>Returns all non-cancelled schedules for a class, ordered by start time.</summary>
    Task<IReadOnlyList<Schedule>> GetSchedulesAsync(long classId, CancellationToken ct = default);

    /// <summary>Returns ACTIVE materials for a class, ordered by creation date descending.</summary>
    Task<IReadOnlyList<Material>> GetMaterialsAsync(long classId, CancellationToken ct = default);

    /// <summary>Returns true if the student is an ACTIVE member of the class.</summary>
    Task<bool> IsEnrolledAsync(long classId, long studentId, CancellationToken ct = default);
}

