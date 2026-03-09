using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Teacher.Profile;

namespace BusinessLogicLayer.Services.Interfaces.Teacher.Profile;

public interface ITeacherProfileService
{
    Task<ApiResponse<object>> UpdateProfileAsync(int teacherId, UpdateTeacherProfileDto dto);
    // Add other profile-specific methods
}
