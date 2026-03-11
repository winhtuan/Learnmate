namespace BusinessLogicLayer.DTOs.Class;

public record ClassListItemDto(
    long Id,
    string Name,
    string Subject,
    string? Description,
    string TeacherName,
    string Status,
    DateTime? NextSessionLocal
);

public record ClassDetailDto(
    long Id,
    string Name,
    string Subject,
    string? Description,
    string Status,
    string TeacherName,
    string? TeacherAvatarUrl,
    string? TeacherBio,
    string TeacherSubjects,
    IReadOnlyList<ClassAssignmentDto> ActiveAssignments,
    IReadOnlyList<ClassScheduleItemDto> UpcomingSchedules,
    IReadOnlyList<ClassMaterialDto> RecentMaterials
);

public record ClassAssignmentDto(
    long Id,
    string Title,
    string? Description,
    DateTime? DueDateLocal,
    string SubmissionStatus, // "not_started" | "in_progress" | "submitted" | "graded" | "missing"
    decimal? Score,
    int? TotalQuestions,
    decimal? TotalPoints
);

public record ClassScheduleItemDto(
    long Id,
    string Title,
    DateTime StartTimeLocal,
    DateTime EndTimeLocal,
    string Type,
    string Status
);

public record ClassMaterialDto(
    long Id,
    string Title,
    string? Description,
    string FileType,
    string FileUrl,
    DateTime UploadedAtLocal,
    long? FileSizeBytes
);
