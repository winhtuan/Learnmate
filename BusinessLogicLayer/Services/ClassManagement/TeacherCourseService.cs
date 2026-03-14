using BusinessLogicLayer.DTOs.Teacher.Classes;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Models;
using DataAccessLayer.Repositories.Interfaces;

namespace BusinessLogicLayer.Services;

public class TeacherCourseService(ITeacherCourseRepository classRepo) : ITeacherCourseService
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

    public async Task<TeacherClassDetailDto?> GetTeacherClassDetailAsync(long classId, long teacherId, CancellationToken ct = default)
    {
        var cls = await classRepo.GetTeacherClassDetailAsync(classId, teacherId, ct);
        if (cls is null) return null;
        int studentCount = cls.ClassMembers.Count;
        return new TeacherClassDetailDto
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
            Materials = cls.Materials
                .Select(m => new TeacherMaterialItemDto
                {
                    Id          = m.Id,
                    Title       = m.Title,
                    Description = m.Description,
                    FileUrl     = m.FileUrl,
                    FileType    = m.FileType,
                    Status      = m.Status.ToString(),
                    UploadedAt  = m.CreatedAt.ToLocalTime()
                })
                .ToList()
        };
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
}
