namespace BusinessLogicLayer.DTOs.Tutor;

/// <summary>
/// Chỉ chứa các trường thực sự tồn tại trong TeacherProfile + User.
/// Các trường UI-only (IsVerified, Location, TotalLessons, AvailableSlots)
/// được xử lý ở Blazor layer với giá trị mặc định.
/// </summary>
public record TutorSummaryDto(
    long TeacherUserId,
    string FullName,
    string? AvatarUrl,
    string Subjects,
    decimal HourlyRate,
    double RatingAvg,
    int TotalRatingCount,
    string? Bio
);

public record CreateBookingRequestDto
{
    public long TeacherId { get; set; }
    public DateTime RequestedStartTime { get; set; }
    public DateTime RequestedEndTime { get; set; }
    public string? Note { get; set; }
}

public record BookingRequestDto(
    long Id,
    long TeacherId,
    string TeacherName,
    string? TeacherAvatarUrl,
    DateTime RequestedStartTimeLocal,
    DateTime RequestedEndTimeLocal,
    string Status,
    string? Note,
    DateTime CreatedAtLocal
);
