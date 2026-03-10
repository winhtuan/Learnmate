using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IClassManagementService
{
    Task<IEnumerable<DTOs.ClassManagement.ClassRowDto>> GetClassesAsync(string statusFilter = "", string searchQuery = "");
    Task<ApiResponse<object?>> CreateClassAsync(DTOs.ClassManagement.NewClassRequestDto request);
    Task<ApiResponse<object?>> ChangeClassStatusAsync(long classId, string newStatus);
    Task<ApiResponse<object?>> DeleteClassAsync(long classId);
    Task<IEnumerable<DTOs.ClassManagement.TeacherDropdownDto>> GetAvailableTeachersAsync();
}
