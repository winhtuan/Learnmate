using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ScheduleRepository(AppDbContext db) : IScheduleRepository
{
    public async Task<IReadOnlyList<Schedule>> GetWeeklyForStudentAsync(
        long studentId, DateTime weekStartUtc, DateTime weekEndUtc, CancellationToken ct = default) =>
        await db.Schedules
            .AsNoTracking()
            .Include(s => s.Class)
            .Where(s => s.StartTime >= weekStartUtc
                     && s.StartTime <= weekEndUtc
                     && s.Status    != ScheduleStatus.CANCELLED
                     && s.Class.DeletedAt == null
                     && s.Class.Status    == ClassStatus.ACTIVE
                     && s.Class.ClassMembers.Any(m => m.StudentId == studentId
                                                   && m.Status == ClassMemberStatus.ACTIVE))
            .OrderBy(s => s.StartTime)
            .ToListAsync(ct);
}
