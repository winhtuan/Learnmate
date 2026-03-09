namespace BusinessLogicLayer.DTOs.Student;

public record StudentDashboardDto(
    string FullName,
    string CurrentTerm,
    StudentStatsDto Stats,
    IReadOnlyList<DayScheduleDto> WeekSchedule,
    IReadOnlyList<EnrolledClassDto> Classes,
    IReadOnlyList<NotificationDto> Notifications
);

public record StudentStatsDto(
    int ActiveClasses,
    int PendingTasks,
    int StudyStreakDays,
    NextSessionDto? NextSession
);

public record NextSessionDto(string ClassName, string Room, DateTime StartTimeLocal);

public record DayScheduleDto(
    DateOnly Date,
    IReadOnlyList<ScheduledClassDto> Classes
);

public record ScheduledClassDto(
    string ClassName,
    string Room,
    TimeOnly StartTime,
    TimeOnly EndTime
);

public record EnrolledClassDto(
    long ClassId,
    string ClassName,
    string SubjectColor,
    int ProgressPercent,
    string? NextTaskTitle,
    DateOnly? NextTaskDue
);

public record NotificationDto(
    long Id,
    string Message,
    DateTime CreatedAt,
    bool IsRead
);

public record StudentProfileHeaderDto(
    string FullName,
    string? AvatarUrl,
    string? GradeLevel
);
