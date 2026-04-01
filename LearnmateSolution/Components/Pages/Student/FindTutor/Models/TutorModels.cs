namespace LearnmateSolution.Components.Pages.Student.FindTutor.Models;

public sealed record TutorListItem(
    long Id,
    string Name,
    string AvatarUrl,
    bool IsVerified,
    bool IsSuperTutor,
    double Rating,
    int ReviewCount,
    int StudentCount,
    string Bio,
    string[] Subjects,
    decimal HourlyRate,
    AvailabilityStatus Availability,
    string? AvailabilityLabel,
    string Location,
    string ResponseTime,
    int TotalLessons,
    string FullBio,
    string[] AvailableSlots,
    BusinessLogicLayer.DTOs.Tutor.ClassScheduleDto[] Schedules,
    DateTime? StartDate,
    DateTime? EndDate,
    int? TotalSessions
);

public sealed record ClassListItem(
    long ClassId,
    string ClassName,
    string? Description,
    string Subject,
    string ScheduleSummary,   // e.g. "Mon/Wed/Fri  18:00–20:00", "" nếu chưa có
    int EnrolledCount,
    int MaxStudents,
    // Thông tin gia sư — dùng cho QuickView, booking, chat
    long TeacherId,
    string TeacherName,
    string TeacherAvatarUrl,
    double Rating,
    int ReviewCount,
    string[] Subjects,
    decimal HourlyRate,
    string Bio,
    BusinessLogicLayer.DTOs.Tutor.ClassScheduleDto[] Schedules,
    DateTime? StartDate,
    DateTime? EndDate,
    int? TotalSessions
);

public enum AvailabilityStatus { AvailableToday, NextSlot, FullyBooked }
