using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ClassRepository(AppDbContext db) : IClassRepository
{
    public async Task<IReadOnlyList<Class>> GetEnrolledWithDetailsAsync(
        long studentId,
        CancellationToken ct = default
    )
    {
        var nowUtc = DateTime.UtcNow;

        return await db
            .ClassMembers.AsNoTracking()
            .Where(m =>
                m.StudentId == studentId
                && m.Status == ClassMemberStatus.ACTIVE
                && m.Class.DeletedAt == null
                && m.Class.Status == ClassStatus.ACTIVE
            )
            .Include(m => m.Class)
            .ThenInclude(c => c.Teacher)
            .ThenInclude(t => t.TeacherProfile)
            .Include(m => m.Class)
            .ThenInclude(c =>
                c.Schedules.Where(s => s.Status != ScheduleStatus.CANCELLED && s.StartTime > nowUtc)
                    .OrderBy(s => s.StartTime)
                    .Take(1)
            )
            .Select(m => m.Class)
            .ToListAsync(ct);
    }

    public async Task<Class?> GetByIdWithDetailsAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    )
    {
        var nowUtc = DateTime.UtcNow;

        // Single query: enrollment check is folded into the WHERE clause
        return await db
            .Classes.AsNoTracking()
            .Where(c =>
                c.Id == classId
                && c.DeletedAt == null
                && c.ClassMembers.Any(m =>
                    m.StudentId == studentId && m.Status == ClassMemberStatus.ACTIVE
                )
            )
            .Include(c => c.Teacher)
            .ThenInclude(t => t.TeacherProfile)
            .Include(c =>
                c.Assignments.Where(a =>
                        a.DeletedAt == null && a.Status == AssignmentStatus.PUBLISHED
                    )
                    .OrderBy(a => a.DueDate)
            )
            .ThenInclude(a =>
                a.Submissions.Where(s => s.StudentId == studentId && s.DeletedAt == null).Take(1)
            )
            .Include(c =>
                c.Schedules.Where(s => s.Status != ScheduleStatus.CANCELLED && s.StartTime > nowUtc)
                    .OrderBy(s => s.StartTime)
                    .Take(5)
            )
            .Include(c =>
                c.Materials.Where(m => m.Status == MaterialStatus.ACTIVE)
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(5)
            )
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<(Assignment, Submission?)>> GetAssignmentsWithSubmissionsAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    )
    {
        var assignments = await db
            .Assignments.AsNoTracking()
            .Where(a =>
                a.ClassId == classId
                && a.DeletedAt == null
                && a.Status == AssignmentStatus.PUBLISHED
            )
            .OrderBy(a => a.DueDate)
            .Include(a =>
                a.Submissions.Where(s => s.StudentId == studentId && s.DeletedAt == null).Take(1)
            )
            .ToListAsync(ct);

        return assignments.Select(a => (a, a.Submissions.FirstOrDefault())).ToList();
    }

    public async Task<IReadOnlyList<Schedule>> GetSchedulesAsync(
        long classId,
        CancellationToken ct = default
    ) =>
        await db
            .Schedules.AsNoTracking()
            .Where(s => s.ClassId == classId && s.Status != ScheduleStatus.CANCELLED)
            .OrderBy(s => s.StartTime)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Material>> GetMaterialsAsync(
        long classId,
        CancellationToken ct = default
    ) =>
        await db
            .Materials.AsNoTracking()
            .Where(m => m.ClassId == classId && m.Status == MaterialStatus.ACTIVE)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(ct);

    public async Task<bool> IsEnrolledAsync(
        long classId,
        long studentId,
        CancellationToken ct = default
    ) =>
        await db.ClassMembers.AnyAsync(
            m =>
                m.ClassId == classId
                && m.StudentId == studentId
                && m.Status == ClassMemberStatus.ACTIVE,
            ct
        );

    public async Task<Submission?> GetSubmissionAsync(
        long assignmentId,
        long studentId,
        CancellationToken ct = default
    ) =>
        await db.Submissions.FirstOrDefaultAsync(
            s => s.AssignmentId == assignmentId && s.StudentId == studentId && s.DeletedAt == null,
            ct
        );

    public async Task<Submission> UpsertSubmissionAsync(
        Submission submission,
        CancellationToken ct = default
    )
    {
        if (submission.Id == 0)
            db.Submissions.Add(submission);
        else
            db.Submissions.Update(submission);

        await db.SaveChangesAsync(ct);
        return submission;
    }

    public async Task<Material> CreateMaterialAsync(
        Material material,
        CancellationToken ct = default
    )
    {
        db.Materials.Add(material);
        await db.SaveChangesAsync(ct);
        return material;
    }

    public async Task<IReadOnlyList<Schedule>> GetSchedulesWithVideosAsync(
        long classId,
        CancellationToken ct = default
    ) =>
        await db
            .Schedules.AsNoTracking()
            .Where(s => s.ClassId == classId && s.Status != ScheduleStatus.CANCELLED)
            .Include(s => s.VideoSession)
            .OrderByDescending(s => s.StartTime)
            .ToListAsync(ct);
}
