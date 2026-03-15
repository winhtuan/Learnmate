using BCrypt.Net;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Teacher.Profile;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Repositories.Interfaces;

namespace BusinessLogicLayer.Services;

public class TeacherProfileService(
    ITeacherProfileRepository profileRepo,
    IUserRepository userRepo) : ITeacherProfileService
{
    public async Task<TeacherProfileDto?> GetProfileAsync(long userId)
    {
        var profile = await profileRepo.GetByUserIdAsync(userId);
        if (profile is null) return null;

        return new TeacherProfileDto
        {
            UserId           = profile.UserId,
            Email            = profile.User?.Email ?? string.Empty,
            FullName         = profile.FullName,
            AvatarUrl        = profile.AvatarUrl,
            Bio              = profile.Bio,
            Subjects         = profile.Subjects,
            HourlyRate       = profile.HourlyRate,
            LanguagesSpoken  = profile.LanguagesSpoken,
            YearsOfExperience = profile.YearsOfExperience,
            TeachingPhilosophy = profile.TeachingPhilosophy,
            BankAccount      = profile.BankAccount,
            RatingAvg        = profile.RatingAvg,
            TotalRatingCount = profile.TotalRatingCount,
            Status           = profile.Status,
            AdminNotes       = profile.AdminNotes
        };
    }

    public async Task<ApiResponse<object>> UpdateProfileAsync(long userId, UpdateTeacherProfileDto dto)
    {
        var profile = await profileRepo.GetByUserIdAsync(userId);
        if (profile is null)
        {
            // Fallback for existing users who registered before the fix
            profile = new TeacherProfile
            {
                UserId = userId,
                FullName = dto.FullName ?? "",
                Status = ComplianceStatus.NONE,
                Subjects = string.IsNullOrWhiteSpace(dto.Subjects) ? "Not Specified" : dto.Subjects,
                HourlyRate = dto.HourlyRate > 0 ? dto.HourlyRate : 1
            };
            await profileRepo.AddAsync(profile);
        }

        profile.FullName    = dto.FullName;
        profile.Bio         = dto.Bio;
        profile.Subjects    = dto.Subjects;
        profile.HourlyRate  = dto.HourlyRate;
        profile.LanguagesSpoken = dto.LanguagesSpoken;
        profile.YearsOfExperience = dto.YearsOfExperience;
        profile.TeachingPhilosophy = dto.TeachingPhilosophy;
        profile.BankAccount = dto.BankAccount;
        profile.AvatarUrl   = dto.AvatarUrl;

        await profileRepo.UpdateAsync(profile);

        // Cập nhật AvatarUrl trên bảng User nếu có
        if (!string.IsNullOrWhiteSpace(dto.AvatarUrl))
        {
            var user = await userRepo.GetByIdAsync(userId);
            if (user is not null)
            {
                user.AvatarUrl = dto.AvatarUrl;
                await userRepo.UpdateAsync(user);
            }
        }

        return ApiResponse<object>.Ok(null, "Cập nhật hồ sơ thành công.");
    }

    public async Task<ApiResponse<object>> ChangePasswordAsync(long userId, ChangePasswordDto dto)
    {
        var user = await userRepo.GetByIdAsync(userId);
        if (user is null)
            return ApiResponse<object>.Fail("Không tìm thấy tài khoản.");

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            return ApiResponse<object>.Fail("Mật khẩu hiện tại không chính xác.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await userRepo.UpdateAsync(user);

        return ApiResponse<object>.Ok(null, "Đổi mật khẩu thành công.");
    }
}
