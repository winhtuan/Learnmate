using BusinessLogicLayer.DTOs.Teacher.Classes;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Repositories.Interfaces;

namespace BusinessLogicLayer.Services;

public class TeacherCourseService(
    ITeacherCourseRepository classRepo,
    IStudentClassMemberRepository memberRepo,
    IUserRepository userRepo,
    IFileStorageService fileStorage
) : ITeacherCourseService
{
    // ── Allowed file extensions for materials ────────────────────────────────
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",
        ".jpg", ".jpeg", ".png", ".gif", ".zip", ".rar", ".txt"
    };

    private const long MaxFileSizeBytes = 50 * 1024 * 1024; // 50 MB

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
            Status = ClassStatus.ACTIVE
        };
        var created = await classRepo.CreateAsync(cls);
        return created.Id;
    }

    public async Task<TeacherClassDetailDto?> GetTeacherClassDetailAsync(long classId, long teacherId, CancellationToken ct = default)
    {
        var cls = await classRepo.GetTeacherClassDetailAsync(classId, teacherId, ct);
        if (cls is null) return null;
        int studentCount = cls.ClassMembers.Count;
        var dto = new TeacherClassDetailDto
        {
            Id              = cls.Id,
            Name            = cls.Name,
            Subject         = cls.Subject,
            Description     = cls.Description,
            Status          = cls.Status,
            MaxStudents     = cls.MaxStudents,
            ThumbnailUrl    = cls.ThumbnailUrl,
            CreatedAt       = cls.CreatedAt,
            TotalStudents   = studentCount,
            TotalAssignments = cls.Assignments.Count,
            TotalSchedules  = cls.Schedules.Count,
            TotalMaterials  = cls.Materials.Count,
            Students = cls.ClassMembers
                .Select(m => new TeacherStudentItemDto
                {
                    StudentId  = m.StudentId,
                    FullName   = m.Student?.StudentProfile?.FullName ?? m.Student?.Email ?? "—",
                    Email      = m.Student?.Email ?? "—",
                    AvatarUrl  = m.Student?.AvatarUrl,
                    GradeLevel = m.Student?.StudentProfile?.GradeLevel,
                    Status     = m.Status.ToString(),
                    JoinedAt   = m.JoinedAt
                })
                .OrderBy(s => s.FullName)
                .ToList(),
            Assignments = cls.Assignments
                .Select(a => new TeacherAssignmentItemDto
                {
                    Id          = a.Id,
                    Title       = a.Title,
                    Description = a.Description,
                    Status      = a.Status.ToString(),
                    DueDate     = a.DueDate?.ToLocalTime(),
                    Submitted   = a.Submissions.Count,
                    Total       = studentCount
                })
                .ToList(),
            Schedules = cls.Schedules
                .OrderByDescending(s => s.StartTime)
                .Select(s => new TeacherScheduleItemDto
                {
                    Id        = s.Id,
                    Title     = s.Title,
                    StartTime = s.StartTime.ToLocalTime(),
                    EndTime   = s.EndTime.ToLocalTime(),
                    Type      = s.Type.ToString(),
                    Status    = s.Status.ToString(),
                    IsTrial   = s.IsTrial
                })
                .ToList(),
            Materials = []
        };

        foreach(var m in cls.Materials)
        {
            dto.Materials.Add(new TeacherMaterialItemDto
            {
                Id          = m.Id,
                Title       = m.Title,
                Description = m.Description,
                FileUrl     = await fileStorage.GetUrlAsync(m.FileUrl, ct: ct),
                FileType    = m.FileType,
                FileSizeBytes = m.FileSizeBytes,
                Status      = m.Status.ToString(),
                UploadedAt  = m.CreatedAt.ToLocalTime()
            });
        }

        return dto;
    }

    public async Task<List<TeacherClassListItemDto>> GetTeacherClassesAsync(long teacherId, CancellationToken ct = default)
    {
        var classes = await classRepo.GetByTeacherIdAsync(teacherId, ct);
        return classes.Select(c =>
        {
            var next = c.Schedules.FirstOrDefault();
            return new TeacherClassListItemDto
            {
                Id             = c.Id,
                Name           = c.Name,
                Subject        = c.Subject,
                Description    = c.Description,
                Status         = c.Status,
                StudentCount   = c.ClassMembers.Count,
                MaxStudents    = c.MaxStudents,
                ThumbnailUrl   = c.ThumbnailUrl,
                CreatedAt      = c.CreatedAt,
                NextSessionUtc = next?.StartTime,
                ScheduleLabel  = next is not null
                    ? next.StartTime.ToLocalTime().ToString("ddd, MMM d • hh:mm tt")
                    : null
            };
        }).ToList();
    }

    // ── Student Management ────────────────────────────────────────────────────
    public async Task<StudentSearchResultDto?> SearchStudentByEmailAsync(string email, long classId)
    {
        var user = await userRepo.GetByEmailAsync(email.Trim().ToLower());
        if (user is null) return null;

        var existing = await memberRepo.GetMemberAsync(classId, user.Id);
        return new StudentSearchResultDto
        {
            UserId         = user.Id,
            FullName       = user.StudentProfile?.FullName ?? user.Email,
            Email          = user.Email,
            AvatarUrl      = user.AvatarUrl,
            GradeLevel     = user.StudentProfile?.GradeLevel,
            AlreadyInClass = existing is not null && existing.Status == ClassMemberStatus.ACTIVE
        };
    }

    public async Task<string?> AddStudentToClassAsync(
        long classId, long teacherId, string studentEmail)
    {
        var cls = await classRepo.GetTeacherClassDetailAsync(classId, teacherId);
        if (cls is null) return "Không tìm thấy lớp học.";

        var user = await userRepo.GetByEmailAsync(studentEmail.Trim().ToLower());
        if (user is null) return "Không tìm thấy tài khoản với email này.";

        var activeCount = cls.ClassMembers.Count(m => m.Status == ClassMemberStatus.ACTIVE);
        if (activeCount >= cls.MaxStudents)
            return $"Lớp đã đủ sĩ số ({cls.MaxStudents} học sinh).";

        var existing = await memberRepo.GetMemberAsync(classId, user.Id);
        if (existing is not null)
        {
            if (existing.Status == ClassMemberStatus.ACTIVE)
                return "Học sinh này đã có trong lớp.";
            existing.Status   = ClassMemberStatus.ACTIVE;
            existing.JoinedAt = DateTime.UtcNow;
            await memberRepo.AddToClassAsync(existing);
            return null;
        }

        await memberRepo.AddToClassAsync(new ClassMember
        {
            ClassId   = classId,
            StudentId = user.Id,
            Status    = ClassMemberStatus.ACTIVE,
            JoinedAt  = DateTime.UtcNow
        });
        return null;
    }

    public async Task<bool> RemoveStudentFromClassAsync(
        long classId, long teacherId, long studentId)
    {
        var cls = await classRepo.GetTeacherClassDetailAsync(classId, teacherId);
        if (cls is null) return false;
        return await memberRepo.RemoveFromClassAsync(classId, studentId);
    }

    // ── Upload Material ──────────────────────────────────────────────────────

    public async Task<TeacherMaterialItemDto> UploadMaterialAsync(
        long classId, long teacherId,
        string fileName, string title, string? description,
        Stream fileContent, string contentType, long fileSizeBytes,
        CancellationToken ct = default)
    {
        // Validate file extension
        var ext = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext))
            throw new InvalidOperationException(
                $"Định dạng file '{ext}' không được hỗ trợ. Chỉ chấp nhận: {string.Join(", ", AllowedExtensions)}");

        // Validate file size
        if (fileSizeBytes > MaxFileSizeBytes)
            throw new InvalidOperationException(
                $"File quá lớn ({fileSizeBytes / 1024 / 1024} MB). Tối đa: {MaxFileSizeBytes / 1024 / 1024} MB");

        // Generate unique object path in MinIO
        var fileType = ext.TrimStart('.').ToLower();
        var objectPath = $"materials/{classId}/{Guid.NewGuid():N}{ext.ToLower()}";

        // Upload to cloud storage (MinIO)
        await fileStorage.UploadAsync(objectPath, fileContent, contentType, ct);

        // Save metadata to DB
        var material = new Material
        {
            ClassId       = classId,
            UploadedBy    = teacherId,
            Title         = title.Trim(),
            Description   = description?.Trim(),
            FileUrl       = objectPath,
            FileType      = fileType,
            FileSizeBytes = fileSizeBytes,
            Status        = MaterialStatus.ACTIVE
        };

        var saved = await classRepo.AddMaterialAsync(material, ct);

        return new TeacherMaterialItemDto
        {
            Id            = saved.Id,
            Title         = saved.Title,
            Description   = saved.Description,
            FileUrl       = await fileStorage.GetUrlAsync(objectPath, ct: ct),
            FileType      = saved.FileType,
            FileSizeBytes = saved.FileSizeBytes,
            Status        = saved.Status.ToString(),
            UploadedAt    = saved.CreatedAt.ToLocalTime()
        };
    }

    // ── Delete Material ──────────────────────────────────────────────────────
    public async Task DeleteMaterialAsync(long classId, long teacherId, long materialId, CancellationToken ct = default)
    {
        // Verify class belongs to teacher
        var cls = await classRepo.GetTeacherClassDetailAsync(classId, teacherId, ct);
        if (cls is null)
            throw new InvalidOperationException("Không tìm thấy lớp học.");

        var material = cls.Materials.FirstOrDefault(m => m.Id == materialId);
        if (material is null)
            throw new InvalidOperationException("Không tìm thấy tài liệu.");

        // Delete from cloud storage
        try { await fileStorage.DeleteAsync(material.FileUrl, ct); }
        catch { /* file may already be missing */ }

        // Soft-delete in database
        await classRepo.DeleteMaterialAsync(materialId, classId, ct);
    }

    // ── Assignments & Grading ────────────────────────────────────────────────
    public async Task<TeacherAssignmentFullDetailDto?> GetAssignmentDetailAsync(long assignmentId, long classId, long teacherId, CancellationToken ct = default)
    {
        var a = await classRepo.GetAssignmentDetailAsync(assignmentId, classId, teacherId, ct);
        if (a is null) return null;

        var dto = new TeacherAssignmentFullDetailDto
        {
            Id = a.Id,
            Title = a.Title,
            Description = a.Description,
            Status = a.Status.ToString(),
            DueDate = a.DueDate,
            TotalStudents = a.Class.MaxStudents, // Note: ideally we count active students in class here.
        };

        foreach (var s in a.Submissions)
        {
            var fileName = s.FileUrl;
            var completeFileUrl = s.FileUrl;
            if (!string.IsNullOrEmpty(fileName))
            {
                try {
                    var uri = new Uri(fileName);
                    fileName = Path.GetFileName(uri.LocalPath);
                } catch { /* ignored */ }
                
                try {
                    // Always try to fetch absolute URL from storage interface.
                    completeFileUrl = await fileStorage.GetUrlAsync(s.FileUrl, ct: ct);
                } catch { /* ignored */ }
            }
            
            dto.Submissions.Add(new TeacherSubmissionItemDto
            {
                Id = s.Id,
                StudentId = s.StudentId,
                StudentName = s.Student?.StudentProfile?.FullName ?? s.Student?.Email ?? "Unknown",
                StudentAvatarUrl = s.Student?.AvatarUrl,
                Status = s.Status.ToString(),
                SubmittedAt = s.SubmittedAt,
                FileUrl = completeFileUrl,
                FileName = fileName,
                Score = s.Score,
                FeedbackComment = s.Feedback?.Comment
            });
        }

        dto.SubmittedCount = a.Submissions.Count(s => s.Status == SubmissionStatus.SUBMITTED || s.Status == SubmissionStatus.GRADED);
        dto.GradedCount = a.Submissions.Count(s => s.Status == SubmissionStatus.GRADED);

        return dto;
    }

    public async Task GradeSubmissionAsync(long submissionId, long teacherId, decimal score, string? feedback, CancellationToken ct = default)
    {
        var s = await classRepo.GradeSubmissionAsync(submissionId, teacherId, score, feedback, ct);
        if (s is null)
            throw new InvalidOperationException("Không tìm thấy bài tập hoặc bạn không có quyền chấm bài này.");
    }
}
