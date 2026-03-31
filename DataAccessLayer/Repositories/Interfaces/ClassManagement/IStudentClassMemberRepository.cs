using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IStudentClassMemberRepository
{
    /// <summary>Trả về danh sách ClassMember (kèm Class và assignment gần nhất sắp tới) của student.
    /// Chỉ lấy các lớp ACTIVE mà student đang ACTIVE.</summary>
    Task<IReadOnlyList<ClassMember>> GetEnrolledWithClassAsync(
        long studentId,
        CancellationToken ct = default
    );

    /// <summary>Checks if the student is already a member of the class (any status).</summary>
    Task<ClassMember?> GetMemberAsync(long classId, long studentId, CancellationToken ct = default);

    /// <summary>Adds a student to the class as ACTIVE.</summary>
    Task AddToClassAsync(ClassMember member, CancellationToken ct = default);

    /// <summary>Sets the member status to DROPPED. Returns false if not found or already inactive.</summary>
    Task<bool> LeaveClassAsync(long classId, long studentId, CancellationToken ct = default);

    /// <summary>Teacher removes a student: sets status to DROPPED. Returns false if not found.</summary>
    Task<bool> RemoveFromClassAsync(long classId, long studentId, CancellationToken ct = default);
}
