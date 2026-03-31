using System.ComponentModel.DataAnnotations;
using BusinessObject.Enum;

namespace BusinessLogicLayer.DTOs.Teacher.Classes;

/// <summary>DTO hiển thị 1 lớp trong danh sách My Classes của giáo viên.</summary>
public class TeacherClassListItemDto
{
    public long        Id             { get; set; }
    public string      Name           { get; set; } = string.Empty;
    public string      Subject        { get; set; } = string.Empty;
    public string?     Description    { get; set; }
    public ClassStatus Status         { get; set; }
    public int         StudentCount   { get; set; }
    public int         MaxStudents    { get; set; }
    public string?     ThumbnailUrl   { get; set; }
    public string?     MeetingLink    { get; set; }
    public DateTime    CreatedAt      { get; set; }
    public DateTime?   NextSessionUtc { get; set; }
    public string?     ScheduleLabel  { get; set; }
}

/// <summary>DTO để tạo lớp học mới.</summary>
public class CreateClassDto
{
    [Required(ErrorMessage = "Tên lớp không được để trống")]
    [MaxLength(200, ErrorMessage = "Tên lớp tối đa 200 ký tự")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Môn học không được để trống")]
    [MaxLength(100, ErrorMessage = "Môn học tối đa 100 ký tự")]
    public string Subject { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Số học sinh tối đa không được để trống")]
    [Range(1, 200, ErrorMessage = "Số học sinh phải từ 1 đến 200")]
    public int MaxStudents { get; set; } = 30;

    public string? ThumbnailUrl { get; set; }
    public string? MeetingLink { get; set; }
}

