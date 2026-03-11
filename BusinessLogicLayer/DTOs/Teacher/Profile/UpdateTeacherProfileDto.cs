using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTOs.Teacher.Profile;

public class UpdateTeacherProfileDto
{
    [Required(ErrorMessage = "Họ tên không được để trống")]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    public string? Bio { get; set; }

    [Required(ErrorMessage = "Môn dạy không được để trống")]
    [MaxLength(500)]
    public string Subjects { get; set; } = string.Empty;

    [Required(ErrorMessage = "Học phí không được để trống")]
    [Range(1, 10_000_000, ErrorMessage = "Học phí phải lớn hơn 0")]
    public decimal HourlyRate { get; set; }

    [MaxLength(100)]
    public string? BankAccount { get; set; }

    public string? AvatarUrl { get; set; }
}

public class ChangePasswordDto
{
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
    [MinLength(6, ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới")]
    [Compare(nameof(NewPassword), ErrorMessage = "Mật khẩu xác nhận không khớp")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}

