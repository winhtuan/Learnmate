using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IClassMemberRepository
{
    /// <summary>
    /// Trả về danh sách ClassMember (kèm Class và assignment gần nhất sắp tới) của student.
    /// Chỉ lấy các lớp ACTIVE mà student đang ACTIVE.
    /// </summary>
    Task<IReadOnlyList<ClassMember>> GetEnrolledWithClassAsync(
        long studentId,
        CancellationToken ct = default
    );

    /// <summary>Sets the member status to DROPPED. Returns false if not found or already inactive.</summary>
    Task<bool> LeaveClassAsync(long classId, long studentId, CancellationToken ct = default);
}
