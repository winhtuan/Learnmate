using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Teacher.Profile;

namespace BusinessLogicLayer.Services.Interfaces;

public interface ITeacherProfileService
{
    Task<TeacherProfileDto?> GetProfileAsync(long userId);
    Task<ApiResponse<object>> UpdateProfileAsync(long userId, UpdateTeacherProfileDto dto);
    Task<ApiResponse<object>> ChangePasswordAsync(long userId, ChangePasswordDto dto);

    /// <summary>
    /// Upload a new avatar image, persist the CDN URL to the user record,
    /// and return the public URL.
    /// </summary>
    Task<ApiResponse<string>> UploadAvatarAsync(
        long userId,
        Stream imageStream,
        string fileName,
        string contentType);
}
