using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Student;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IStudentDashboardService
{
    Task<ApiResponse<StudentDashboardDto>> GetDashboardAsync(
        long userId,
        CancellationToken ct = default
    );
    Task<ApiResponse<StudentProfileHeaderDto>> GetProfileHeaderAsync(
        long userId,
        CancellationToken ct = default
    );
}
