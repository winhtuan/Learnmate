using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Class;
using BusinessLogicLayer.DTOs.Teacher.Classes;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Repositories.Interfaces;

namespace BusinessLogicLayer.Services;

public class ClassService(IClassRepository classRepo) : IClassService
{
    public async Task<long> CreateClassAsync(long teacherId, CreateClassDto dto)
    {
        var cls = new Class
        {
            TeacherId    = teacherId,
            Name         = dto.Name.Trim(),
            Subject      = dto.Subject.Trim(),
            Description  = dto.Description?.Trim(),
            MaxStudents  = dto.MaxStudents,
            ThumbnailUrl = string.IsNullOrWhiteSpace(dto.ThumbnailUrl)
                ? $"https://placehold.co/400?text={Uri.EscapeDataString(dto.Subject)}"
                : dto.ThumbnailUrl.Trim(),
            Status = BusinessObject.Enum.ClassStatus.ACTIVE
        };
        var created = await classRepo.CreateAsync(cls);
        return created.Id;
    }

    public async Task<List<TeacherClassListItemDto>> GetTeacherClassesAsync(
        long teacherId, CancellationToken ct = default)
    {
        var classes = await classRepo.GetByTeacherIdAsync(teacherId, ct);

        return classes.Select(c =>
        {
            var next = c.Schedules.FirstOrDefault();
            return new TeacherClassListItemDto
            {
                Id           = c.Id,
                Name         = c.Name,
                Subject      = c.Subject,
                Description  = c.Description,
                Status       = c.Status,
                StudentCount = c.ClassMembers.Count,
                MaxStudents  = c.MaxStudents,
                ThumbnailUrl = c.ThumbnailUrl,
                CreatedAt    = c.CreatedAt,
                NextSessionUtc = next?.StartTime,
                ScheduleLabel  = next is not null
                    ? next.StartTime.ToLocalTime().ToString("ddd, MMM d • hh:mm tt")
                    : null
            };
        }).ToList();
    }

    public async Task<ApiResponse<IReadOnlyList<ClassListItemDto>>> GetEnrolledClassesAsync(
        long studentId, CancellationToken ct = default)
    {
        var classes = await classRepo.GetEnrolledWithDetailsAsync(studentId, ct);

        var dtos = classes.Select(c =>
        {
            var next = c.Schedules.FirstOrDefault();
            return new ClassListItemDto(
                Id:               c.Id,
                Name:             c.Name,
                Subject:          c.Subject,
                Description:      c.Description,
                TeacherName:      c.Teacher?.TeacherProfile?.FullName ?? c.Teacher?.Email ?? "—",
                Status:           c.Status.ToString(),
                NextSessionLocal: next?.StartTime.ToLocalTime()
            );
        }).ToList();

        return ApiResponse<IReadOnlyList<ClassListItemDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<ClassDetailDto>> GetClassDetailAsync(
        long classId, long studentId, CancellationToken ct = default)
    {
        var cls = await classRepo.GetByIdWithDetailsAsync(classId, studentId, ct);

        if (cls is null)
            return ApiResponse<ClassDetailDto>.Fail("Class not found or access denied.");

        var teacherProfile = cls.Teacher?.TeacherProfile;

        var dto = new ClassDetailDto(
            Id:               cls.Id,
            Name:             cls.Name,
            Subject:          cls.Subject,
            Description:      cls.Description,
            Status:           cls.Status.ToString(),
            TeacherName:      teacherProfile?.FullName ?? cls.Teacher?.Email ?? "—",
            TeacherAvatarUrl: teacherProfile?.AvatarUrl,
            TeacherBio:       teacherProfile?.Bio,
            TeacherSubjects:  teacherProfile?.Subjects ?? "",
            ActiveAssignments: cls.Assignments.Select(a => MapAssignment(a, a.Submissions.FirstOrDefault())).ToList(),
            UpcomingSchedules: cls.Schedules.Select(MapSchedule).ToList(),
            RecentMaterials:   cls.Materials.Select(MapMaterial).ToList()
        );

        return ApiResponse<ClassDetailDto>.Ok(dto);
    }

    public async Task<ApiResponse<IReadOnlyList<ClassAssignmentDto>>> GetClassAssignmentsAsync(
        long classId, long studentId, CancellationToken ct = default)
    {
        if (!await classRepo.IsEnrolledAsync(classId, studentId, ct))
            return ApiResponse<IReadOnlyList<ClassAssignmentDto>>.Fail("Class not found or access denied.");

        var pairs = await classRepo.GetAssignmentsWithSubmissionsAsync(classId, studentId, ct);
        var dtos  = pairs.Select(p => MapAssignment(p.Assignment, p.Submission)).ToList();
        return ApiResponse<IReadOnlyList<ClassAssignmentDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<IReadOnlyList<ClassScheduleItemDto>>> GetClassSchedulesAsync(
        long classId, long studentId, CancellationToken ct = default)
    {
        if (!await classRepo.IsEnrolledAsync(classId, studentId, ct))
            return ApiResponse<IReadOnlyList<ClassScheduleItemDto>>.Fail("Class not found or access denied.");

        var schedules = await classRepo.GetSchedulesAsync(classId, ct);
        var dtos = schedules.Select(MapSchedule).ToList();
        return ApiResponse<IReadOnlyList<ClassScheduleItemDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<IReadOnlyList<ClassMaterialDto>>> GetClassMaterialsAsync(
        long classId, long studentId, CancellationToken ct = default)
    {
        if (!await classRepo.IsEnrolledAsync(classId, studentId, ct))
            return ApiResponse<IReadOnlyList<ClassMaterialDto>>.Fail("Class not found or access denied.");

        var materials = await classRepo.GetMaterialsAsync(classId, ct);
        var dtos = materials.Select(MapMaterial).ToList();
        return ApiResponse<IReadOnlyList<ClassMaterialDto>>.Ok(dtos);
    }

    // ── Mapping ───────────────────────────────────────────────────────────────

    private static ClassAssignmentDto MapAssignment(Assignment a, Submission? sub)
    {
        var nowUtc = DateTime.UtcNow;

        string status;
        if (sub?.Status == SubmissionStatus.GRADED)
            status = "graded";
        else if (sub?.Status == SubmissionStatus.SUBMITTED)
            status = "submitted";
        else if (sub?.Status == SubmissionStatus.DRAFT)
            status = "in_progress";
        else if (a.DueDate.HasValue && a.DueDate.Value < nowUtc)
            status = "missing";
        else
            status = "not_started";

        return new ClassAssignmentDto(
            Id:               a.Id,
            Title:            a.Title,
            Description:      a.Description,
            DueDateLocal:     a.DueDate?.ToLocalTime(),
            SubmissionStatus: status,
            Score:            sub?.Score,
            TotalQuestions:   null   // questions not loaded in this query
        );
    }

    private static ClassScheduleItemDto MapSchedule(Schedule s) => new(
        Id:             s.Id,
        Title:          s.Title,
        StartTimeLocal: s.StartTime.ToLocalTime(),
        EndTimeLocal:   s.EndTime.ToLocalTime(),
        Type:           s.Type.ToString(),
        Status:         s.Status.ToString()
    );

    private static ClassMaterialDto MapMaterial(Material m) => new(
        Id:              m.Id,
        Title:           m.Title,
        Description:     m.Description,
        FileType:        m.FileType,
        FileUrl:         m.FileUrl,
        UploadedAtLocal: m.CreatedAt.ToLocalTime()
    );
}
