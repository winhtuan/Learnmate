using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IScheduleRepository
{
    /// <summary>
    /// Trả về danh sách Schedule (kèm Class) của student trong khoảng UTC cho trước.
    /// Chỉ lấy schedule chưa bị huỷ, thuộc lớp ACTIVE mà student đang ACTIVE.
    /// </summary>
    Task<IReadOnlyList<Schedule>> GetWeeklyForStudentAsync(
        long studentId,
        DateTime weekStartUtc,
        DateTime weekEndUtc,
        CancellationToken ct = default
    );
}
