using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces.Teacher.Schedules;

public interface ITeacherScheduleRepository
{
    Task<List<Class>> GetTeacherClassesAsync(long teacherId);
    Task<Class?> GetClassByIdAsync(long classId, long teacherId);
    Task<List<Schedule>> GetSchedulesByClassAsync(long classId);
    Task<Schedule?> GetScheduleByIdAsync(long scheduleId);
    Task<Schedule> CreateScheduleAsync(Schedule schedule);
    Task BulkCreateSchedulesAsync(List<Schedule> schedules);
    Task UpdateScheduleAsync(Schedule schedule);
    Task DeleteScheduleAsync(Schedule schedule);
    Task<bool> HasOverlappingScheduleAsync(long classId, DateTime startTime, DateTime endTime, long? excludeScheduleId = null);
    
    Task<List<User>> GetClassStudentsAsync(long classId);
    Task<List<Attendance>> GetAttendancesByScheduleAsync(long scheduleId);
    Task SaveAttendancesAsync(List<Attendance> attendancesToAdd, List<Attendance> attendancesToUpdate);
}
