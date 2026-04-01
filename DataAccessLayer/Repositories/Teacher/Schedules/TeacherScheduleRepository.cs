using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces.Teacher.Schedules;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Teacher.Schedules;

public class TeacherScheduleRepository(AppDbContext db) : ITeacherScheduleRepository
{
    public async Task<List<Class>> GetTeacherClassesAsync(long teacherId)
    {
        return await db.Classes
            .Where(c => c.TeacherId == teacherId && c.DeletedAt == null)
            .Include(c => c.ClassMembers)
            .ToListAsync();
    }

    public async Task<Class?> GetClassByIdAsync(long classId, long teacherId)
    {
        return await db.Classes
            .Where(c => c.Id == classId && c.TeacherId == teacherId && c.DeletedAt == null)
            .Include(c => c.ClassMembers)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Schedule>> GetSchedulesByClassAsync(long classId)
    {
        return await db.Schedules
            .Where(s => s.ClassId == classId)
            .Include(s => s.Attendances)
            .OrderByDescending(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<Schedule?> GetScheduleByIdAsync(long scheduleId)
    {
        return await db.Schedules
            .Include(s => s.Attendances)
            .FirstOrDefaultAsync(s => s.Id == scheduleId);
    }

    public async Task<Schedule> CreateScheduleAsync(Schedule schedule)
    {
        db.Schedules.Add(schedule);
        await db.SaveChangesAsync();
        return schedule;
    }

    public async Task BulkCreateSchedulesAsync(List<Schedule> schedules)
    {
        db.Schedules.AddRange(schedules);
        await db.SaveChangesAsync();
    }

    public async Task UpdateScheduleAsync(Schedule schedule)
    {
        db.Schedules.Update(schedule);
        await db.SaveChangesAsync();
    }

    public async Task DeleteScheduleAsync(Schedule schedule)
    {
        db.Schedules.Remove(schedule);
        await db.SaveChangesAsync();
    }

    public async Task<bool> HasOverlappingScheduleAsync(
        long classId,
        DateTime startTime,
        DateTime endTime,
        long? excludeScheduleId = null)
    {
        return await db.Schedules
            .Where(s => s.ClassId == classId
                && (excludeScheduleId == null || s.Id != excludeScheduleId)
                && s.StartTime < endTime
                && s.EndTime > startTime)
            .AnyAsync();
    }

    public async Task<List<User>> GetClassStudentsAsync(long classId)
    {
        var studentIds = await db.ClassMembers
            .Where(cm => cm.ClassId == classId && cm.Status == BusinessObject.Enum.ClassMemberStatus.ACTIVE)
            .Select(cm => cm.StudentId)
            .ToListAsync();

        return await db.Users
            .Include(u => u.StudentProfile)
            .Where(u => studentIds.Contains(u.Id))
            .ToListAsync();
    }

    public async Task<List<Attendance>> GetAttendancesByScheduleAsync(long scheduleId)
    {
        return await db.Attendances
            .Where(a => a.ScheduleId == scheduleId)
            .Include(a => a.Student)
            .ToListAsync();
    }

    public async Task SaveAttendancesAsync(List<Attendance> attendancesToAdd, List<Attendance> attendancesToUpdate)
    {
        if (attendancesToAdd.Any())
        {
            db.Attendances.AddRange(attendancesToAdd);
        }
        
        if (attendancesToUpdate.Any())
        {
            db.Attendances.UpdateRange(attendancesToUpdate);
        }
        
        await db.SaveChangesAsync();
    }
}
