using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Teacher.Profile;

namespace BusinessLogicLayer.Services.Interfaces.Teacher.Profile;

public interface ITeacherProfileService
{
    Task<TeacherProfileDto?> GetProfileAsync(long userId);
    Task<ApiResponse<object>> UpdateProfileAsync(long userId, UpdateTeacherProfileDto dto);
    Task<ApiResponse<object>> ChangePasswordAsync(long userId, ChangePasswordDto dto);
}

