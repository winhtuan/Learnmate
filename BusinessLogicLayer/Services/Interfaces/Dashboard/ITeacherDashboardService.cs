using BusinessLogicLayer.DTOs.Teacher.Dashboard;

namespace BusinessLogicLayer.Services.Interfaces.Dashboard;

public interface ITeacherDashboardService
{
    /// <summary>
    /// Lấy toàn bộ dữ liệu thống kê cho Dashboard của Teacher.
    /// </summary>
    Task<TeacherDashboardDataDto> GetDashboardDataAsync(long teacherId, CancellationToken ct = default);
}
