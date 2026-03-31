using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class StudentClassMemberRepository(AppDbContext db) : IStudentClassMemberRepository
{
    public async Task<IReadOnlyList<ClassMember>> GetEnrolledWithClassAsync(
        long studentId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        return await db.ClassMembers.AsNoTracking()
            .Where(m =>
                m.StudentId == studentId
                && m.Status == ClassMemberStatus.ACTIVE
                && m.Class.DeletedAt == null
                && m.Class.Status == ClassStatus.ACTIVE)
            .Include(m => m.Class)
                .ThenInclude(c => c.Assignments
                    .Where(a => a.Status == AssignmentStatus.PUBLISHED
                             && a.DueDate > now
                             && a.DeletedAt == null)
                    .OrderBy(a => a.DueDate)
                    .Take(1))
            .ToListAsync(ct);
    }

    public async Task<ClassMember?> GetMemberAsync(
        long classId, long studentId, CancellationToken ct = default) =>
        await db.ClassMembers.FirstOrDefaultAsync(
            m => m.ClassId == classId && m.StudentId == studentId, ct);

    public async Task AddToClassAsync(ClassMember member, CancellationToken ct = default)
    {
        db.ClassMembers.Add(member);
        await db.SaveChangesAsync(ct);
    }

    public async Task<bool> LeaveClassAsync(
        long classId, long studentId, CancellationToken ct = default)
    {
        var member = await db.ClassMembers.FirstOrDefaultAsync(
            m => m.ClassId == classId
              && m.StudentId == studentId
              && m.Status == ClassMemberStatus.ACTIVE, ct);
        if (member is null) return false;
        member.Status = ClassMemberStatus.DROPPED;
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> RemoveFromClassAsync(
        long classId, long studentId, CancellationToken ct = default)
    {
        var member = await db.ClassMembers.FirstOrDefaultAsync(
            m => m.ClassId == classId && m.StudentId == studentId, ct);
        if (member is null) return false;
        member.Status = ClassMemberStatus.DROPPED;
        await db.SaveChangesAsync(ct);
        return true;
    }
}
