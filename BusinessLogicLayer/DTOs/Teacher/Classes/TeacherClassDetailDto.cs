using BusinessObject.Enum;

namespace BusinessLogicLayer.DTOs.Teacher.Classes;

/// <summary>Result từ tìm kiếm học sinh theo email trước khi thêm vào lớp.</summary>
public class StudentSearchResultDto
{
    public long    UserId      { get; set; }
    public string  FullName    { get; set; } = string.Empty;
    public string  Email       { get; set; } = string.Empty;
    public string? AvatarUrl   { get; set; }
    public string? GradeLevel  { get; set; }
    public bool    AlreadyInClass { get; set; }
}


// ── Class Detail ──────────────────────────────────────────────────────────────
public class TeacherClassDetailDto
{
    public long        Id             { get; set; }
    public string      Name           { get; set; } = string.Empty;
    public string      Subject        { get; set; } = string.Empty;
    public string?     Description    { get; set; }
    public ClassStatus Status         { get; set; }
    public int         MaxStudents    { get; set; }
    public string?     ThumbnailUrl   { get; set; }
    public DateTime    CreatedAt      { get; set; }

    // Stats
    public int TotalStudents    { get; set; }
    public int TotalAssignments { get; set; }
    public int TotalSchedules   { get; set; }
    public int TotalMaterials   { get; set; }

    // Tab data
    public List<TeacherStudentItemDto>    Students    { get; set; } = [];
    public List<TeacherAssignmentItemDto> Assignments { get; set; } = [];
    public List<TeacherScheduleItemDto>   Schedules   { get; set; } = [];
    public List<TeacherMaterialItemDto>   Materials   { get; set; } = [];
}

// ── Students ──────────────────────────────────────────────────────────────────
public class TeacherStudentItemDto
{
    public long    StudentId  { get; set; }
    public string  FullName   { get; set; } = string.Empty;
    public string  Email      { get; set; } = string.Empty;
    public string? AvatarUrl  { get; set; }
    public string? GradeLevel { get; set; }
    public string  Status     { get; set; } = string.Empty;
    public DateTime JoinedAt  { get; set; }
}

// ── Assignments ───────────────────────────────────────────────────────────────
public class TeacherAssignmentItemDto
{
    public long      Id          { get; set; }
    public string    Title       { get; set; } = string.Empty;
    public string?   Description { get; set; }
    public string    Status      { get; set; } = string.Empty;
    public DateTime? DueDate     { get; set; }
    public int       Submitted   { get; set; }
    public int       Total       { get; set; }
}

// ── Schedules ─────────────────────────────────────────────────────────────────
public class TeacherScheduleItemDto
{
    public long     Id        { get; set; }
    public string   Title     { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime   { get; set; }
    public string   Type      { get; set; } = string.Empty;
    public string   Status    { get; set; } = string.Empty;
    public bool     IsTrial   { get; set; }
}

// ── Materials ─────────────────────────────────────────────────────────────────
public class TeacherMaterialItemDto
{
    public long     Id            { get; set; }
    public string   Title         { get; set; } = string.Empty;
    public string?  Description   { get; set; }
    public string   FileUrl       { get; set; } = string.Empty;
    public string   FileType      { get; set; } = string.Empty;
    public long?    FileSizeBytes { get; set; }
    public string   Status        { get; set; } = string.Empty;
    public DateTime UploadedAt    { get; set; }
}

