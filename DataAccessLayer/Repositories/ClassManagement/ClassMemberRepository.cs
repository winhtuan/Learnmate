using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ClassMemberRepository(AppDbContext db) : IClassMemberRepository
{
    public async Task<IReadOnlyList<ClassMember>> GetEnrolledWithClassAsync(
        long studentId,
        CancellationToken ct = default
    )
    {
        var now = DateTime.UtcNow;

        return await db
            .ClassMembers.AsNoTracking()
            .Where(m =>
                m.StudentId == studentId
                && m.Status == ClassMemberStatus.ACTIVE
                && m.Class.DeletedAt == null
                && m.Class.Status == ClassStatus.ACTIVE
            )
            .Include(m => m.Class)
            .ThenInclude(c =>
                c.Assignments.Where(a =>
                        a.Status == AssignmentStatus.PUBLISHED
                        && a.DueDate > now
                        && a.DeletedAt == null
                    )
                    .OrderBy(a => a.DueDate)
                    .Take(1)
            ) // chỉ cần assignment gần nhất — không load toàn bộ
            .ToListAsync(ct);
    }

    public async Task<bool> LeaveClassAsync(long classId, long studentId, CancellationToken ct = default)
    {
        var member = await db.ClassMembers.FirstOrDefaultAsync(
            m => m.ClassId == classId && m.StudentId == studentId && m.Status == ClassMemberStatus.ACTIVE,
            ct
        );
        if (member is null)
            return false;

        member.Status = ClassMemberStatus.DROPPED;
        await db.SaveChangesAsync(ct);
        return true;
    }
}
