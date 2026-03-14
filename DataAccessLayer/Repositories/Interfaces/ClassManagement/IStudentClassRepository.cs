using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IStudentClassRepository
{
    /// <summary>Returns all ACTIVE classes the student is enrolled in, with teacher profile and next schedule.</summary>
    Task<IReadOnlyList<Class>> GetEnrolledWithDetailsAsync(
        long studentId,
        CancellationToken ct = default
    );

    /// <summary>Returns a single class with full details: teacher, assignments (with student submissions), schedules, materials.</summary>
    Task<Class?> GetByIdWithDetailsAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    );

    /// <summary>Returns assignments for a class with the student's submission.</summary>
    Task<
        IReadOnlyList<(Assignment Assignment, Submission? Submission)>
    > GetAssignmentsWithSubmissionsAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    );

    /// <summary>Returns all non-cancelled schedules for a class, ordered by start time.</summary>
    Task<IReadOnlyList<Schedule>> GetSchedulesAsync(long classId, CancellationToken ct = default);

    /// <summary>Returns ACTIVE materials for a class, ordered by creation date descending.</summary>
    Task<IReadOnlyList<Material>> GetMaterialsAsync(long classId, CancellationToken ct = default);

    /// <summary>Returns true if the student is an ACTIVE member of the class.</summary>
    Task<bool> IsEnrolledAsync(long classId, long studentId, CancellationToken ct = default);

    /// <summary>Returns an existing submission for a student on an assignment, or null.</summary>
    Task<Submission?> GetSubmissionAsync(
        long assignmentId,
        long studentId,
        CancellationToken ct = default
    );

    /// <summary>Creates a new submission or updates an existing one.</summary>
    Task<Submission> UpsertSubmissionAsync(Submission submission, CancellationToken ct = default);

    /// <summary>Inserts a new material record.</summary>
    Task<Material> CreateMaterialAsync(Material material, CancellationToken ct = default);

    /// <summary>Returns all non-cancelled schedules with their VideoSession for a class.</summary>
    Task<IReadOnlyList<Schedule>> GetSchedulesWithVideosAsync(
        long classId,
        CancellationToken ct = default
    );
}
