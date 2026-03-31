using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Class;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Repositories.Interfaces;

namespace BusinessLogicLayer.Services;

public class StudentClassService(
    IStudentClassRepository classRepo,
    IStudentClassMemberRepository classMemberRepo,
    IFileStorageService fileStorage
) : IStudentClassService
{
    public async Task<ApiResponse<IReadOnlyList<ClassListItemDto>>> GetEnrolledClassesAsync(
        long studentId,
        CancellationToken ct = default
    )
    {
        var classes = await classRepo.GetEnrolledWithDetailsAsync(studentId, ct);

        var dtos = classes
            .Select(c =>
            {
                var next = c.Schedules.FirstOrDefault();
                return new ClassListItemDto(
                    Id: c.Id,
                    Name: c.Name,
                    Subject: c.Subject,
                    Description: c.Description,
                    TeacherName: c.Teacher?.TeacherProfile?.FullName ?? c.Teacher?.Email ?? "—",
                    Status: c.Status.ToString(),
                    NextSessionLocal: next?.StartTime.ToLocalTime()
                );
            })
            .ToList();

        return ApiResponse<IReadOnlyList<ClassListItemDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<ClassDetailDto>> GetClassDetailAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    )
    {
        var cls = await classRepo.GetByIdWithDetailsAsync(classId, studentId, ct);

        if (cls is null)
            return ApiResponse<ClassDetailDto>.Fail("Class not found or access denied.");

        var teacherProfile = cls.Teacher?.TeacherProfile;

        var dto = new ClassDetailDto(
            Id: cls.Id,
            Name: cls.Name,
            Subject: cls.Subject,
            Description: cls.Description,
            Status: cls.Status.ToString(),
            TeacherName: teacherProfile?.FullName ?? cls.Teacher?.Email ?? "—",
            TeacherAvatarUrl: teacherProfile?.AvatarUrl,
            TeacherBio: teacherProfile?.Bio,
            TeacherSubjects: teacherProfile?.Subjects ?? "",
            ActiveAssignments: await Task.WhenAll(cls.Assignments.Select(a =>
                    MapAssignmentAsync(a, a.Submissions.FirstOrDefault())
                )),
            UpcomingSchedules: cls.Schedules.Select(MapSchedule).ToList(),
            RecentMaterials: await Task.WhenAll(cls.Materials.Select(MapMaterialAsync)),
            MeetingLink: cls.MeetingLink
        );

        return ApiResponse<ClassDetailDto>.Ok(dto);
    }

    public async Task<ApiResponse<IReadOnlyList<ClassAssignmentDto>>> GetClassAssignmentsAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    )
    {
        if (!await classRepo.IsEnrolledAsync(classId, studentId, ct))
            return ApiResponse<IReadOnlyList<ClassAssignmentDto>>.Fail(
                "Class not found or access denied."
            );

        var pairs = await classRepo.GetAssignmentsWithSubmissionsAsync(classId, studentId, ct);
        var dtos = await Task.WhenAll(pairs.Select(p => MapAssignmentAsync(p.Assignment, p.Submission)));
        return ApiResponse<IReadOnlyList<ClassAssignmentDto>>.Ok(dtos.ToList());
    }

    public async Task<ApiResponse<IReadOnlyList<ClassScheduleItemDto>>> GetClassSchedulesAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    )
    {
        if (!await classRepo.IsEnrolledAsync(classId, studentId, ct))
            return ApiResponse<IReadOnlyList<ClassScheduleItemDto>>.Fail(
                "Class not found or access denied."
            );

        var schedules = await classRepo.GetSchedulesAsync(classId, ct);
        var dtos = schedules.Select(MapSchedule).ToList();
        return ApiResponse<IReadOnlyList<ClassScheduleItemDto>>.Ok(dtos);
    }

    public async Task<ApiResponse<IReadOnlyList<ClassMaterialDto>>> GetClassMaterialsAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    )
    {
        if (!await classRepo.IsEnrolledAsync(classId, studentId, ct))
            return ApiResponse<IReadOnlyList<ClassMaterialDto>>.Fail(
                "Class not found or access denied."
            );

        var materials = await classRepo.GetMaterialsAsync(classId, ct);
        var dtos = await Task.WhenAll(materials.Select(MapMaterialAsync));
        return ApiResponse<IReadOnlyList<ClassMaterialDto>>.Ok(dtos.ToList());
    }

    // ── Student Actions ───────────────────────────────────────────────────────

    public async Task<ApiResponse<bool>> LeaveClassAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    )
    {
        var left = await classMemberRepo.LeaveClassAsync(classId, studentId, ct);
        return left
            ? ApiResponse<bool>.Ok(true, "Left class successfully.")
            : ApiResponse<bool>.Fail("Not enrolled or already left this class.");
    }

    public async Task<ApiResponse<bool>> SubmitAssignmentAsync(
        long classId,
        long assignmentId,
        long studentId,
        Stream? fileStream,
        string? fileName,
        string? contentType,
        CancellationToken ct = default
    )
    {
        if (!await classRepo.IsEnrolledAsync(classId, studentId, ct))
            return ApiResponse<bool>.Fail("Class not found or access denied.");

        var existing = await classRepo.GetSubmissionAsync(assignmentId, studentId, ct);

        if (existing?.Status is SubmissionStatus.SUBMITTED or SubmissionStatus.GRADED)
            return ApiResponse<bool>.Fail("Assignment already submitted.");

        string? fileUrl = null;
        if (fileStream is not null && fileName is not null && contentType is not null)
        {
            var objectPath =
                $"submissions/{assignmentId}/{studentId}/{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{fileName}";
            fileUrl = await fileStorage.UploadAsync(objectPath, fileStream, contentType, ct);
        }

        if (existing is null)
        {
            var submission = new Submission
            {
                AssignmentId = assignmentId,
                StudentId = studentId,
                Status = SubmissionStatus.SUBMITTED,
                SubmittedAt = DateTime.UtcNow,
                FileUrl = fileUrl,
            };
            await classRepo.UpsertSubmissionAsync(submission, ct);
        }
        else
        {
            existing.Status = SubmissionStatus.SUBMITTED;
            existing.SubmittedAt = DateTime.UtcNow;
            if (fileUrl is not null)
                existing.FileUrl = fileUrl;
            await classRepo.UpsertSubmissionAsync(existing, ct);
        }

        return ApiResponse<bool>.Ok(true, "Assignment submitted successfully.");
    }

    public async Task<ApiResponse<ClassMaterialDto>> UploadMaterialAsync(
        long classId,
        long userId,
        string title,
        Stream fileStream,
        string fileName,
        string contentType,
        long? fileSizeBytes = null,
        CancellationToken ct = default
    )
    {
        if (!await classRepo.IsEnrolledAsync(classId, userId, ct))
            return ApiResponse<ClassMaterialDto>.Fail("Class not found or access denied.");

        var objectPath =
            $"materials/{classId}/{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{fileName}";
        var fileUrl = await fileStorage.UploadAsync(objectPath, fileStream, contentType, ct);

        var ext = Path.GetExtension(fileName).TrimStart('.').ToUpperInvariant();
        var material = new Material
        {
            ClassId = classId,
            UploadedBy = userId,
            Title = title,
            FileUrl = fileUrl,
            FileType = string.IsNullOrEmpty(ext) ? "FILE" : ext,
            FileSizeBytes = fileSizeBytes,
            Status = MaterialStatus.ACTIVE,
        };

        var saved = await classRepo.CreateMaterialAsync(material, ct);
        return ApiResponse<ClassMaterialDto>.Ok(
            await MapMaterialAsync(saved),
            "Material uploaded successfully."
        );
    }

    public async Task<ApiResponse<ClassVideosDto>> GetClassVideosAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    )
    {
        if (!await classRepo.IsEnrolledAsync(classId, studentId, ct))
            return ApiResponse<ClassVideosDto>.Fail("Class not found or access denied.");

        var schedules = await classRepo.GetSchedulesWithVideosAsync(classId, ct);

        var items = schedules
            .Where(s => s.VideoSession is not null)
            .Select(s => new VideoSessionItemDto(
                Id: s.VideoSession!.Id,
                ScheduleId: s.Id,
                Title: s.Title,
                MeetingUrl: s.VideoSession.MeetingUrl,
                Provider: s.VideoSession.Provider.ToString(),
                SessionStatus: s.VideoSession.Status.ToString(),
                StartTimeLocal: s.StartTime.ToLocalTime(),
                EndTimeLocal: s.EndTime.ToLocalTime()
            ))
            .ToList();

        var live = items.FirstOrDefault(v => v.SessionStatus == VideoSessionStatus.LIVE.ToString());
        var recorded = items
            .Where(v => v.SessionStatus == VideoSessionStatus.ENDED.ToString())
            .ToList();

        return ApiResponse<ClassVideosDto>.Ok(new ClassVideosDto(live, recorded));
    }

    // ── Mapping ───────────────────────────────────────────────────────────────

    private async Task<ClassAssignmentDto> MapAssignmentAsync(Assignment a, Submission? sub)
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

        var totalPoints = a.Questions.Count > 0 ? a.Questions.Sum(q => q.Points) : (decimal?)null;

        // Resolve assignment file URL
        string? resolvedFileUrl = null;
        if (!string.IsNullOrEmpty(a.FileUrl))
        {
            try
            {
                resolvedFileUrl = a.FileUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    ? a.FileUrl
                    : await fileStorage.GetUrlAsync(a.FileUrl);
            }
            catch { resolvedFileUrl = a.FileUrl; }
        }

        return new ClassAssignmentDto(
            Id: a.Id,
            Title: a.Title,
            Description: a.Description,
            DueDateLocal: a.DueDate?.ToLocalTime(),
            SubmissionStatus: status,
            Score: sub?.Score,
            TotalQuestions: a.Questions.Count > 0 ? a.Questions.Count : null,
            TotalPoints: totalPoints,
            FileUrl: resolvedFileUrl,
            FeedbackComment: sub?.Feedback?.Comment,
            FeedbackScore: sub?.Feedback?.Score
        );
    }

    private static ClassScheduleItemDto MapSchedule(Schedule s) =>
        new(
            Id: s.Id,
            Title: s.Title,
            StartTimeLocal: s.StartTime,
            EndTimeLocal: s.EndTime,
            Type: s.Type.ToString(),
            Status: s.Status.ToString()
        );

    private async Task<ClassMaterialDto> MapMaterialAsync(Material m)
    {
        string resolvedUrl = m.FileUrl;
        try
        {
            if (!string.IsNullOrEmpty(m.FileUrl) && !m.FileUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                resolvedUrl = await fileStorage.GetUrlAsync(m.FileUrl);
        }
        catch { }

        return new ClassMaterialDto(
            Id: m.Id,
            Title: m.Title,
            Description: m.Description,
            FileType: m.FileType,
            FileUrl: resolvedUrl,
            UploadedAtLocal: m.CreatedAt.ToLocalTime(),
            FileSizeBytes: m.FileSizeBytes
        );
    }
}
