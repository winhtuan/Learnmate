using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Teacher.Schedules;
using BusinessLogicLayer.Services.Interfaces.Teacher.Schedules;
using BusinessObject.Models;
using DataAccessLayer.Repositories.Interfaces.Teacher.Schedules;

namespace BusinessLogicLayer.Services.Teacher.Schedules;

public class TeacherScheduleService(ITeacherScheduleRepository repo) : ITeacherScheduleService
{
    public async Task<List<TeacherScheduleClassDto>> GetTeacherClassesAsync(long teacherId)
    {
        var classes = await repo.GetTeacherClassesAsync(teacherId);
        return classes.Select(c => new TeacherScheduleClassDto
        {
            Id = c.Id,
            Name = c.Name,
            Subject = c.Subject,
            Status = c.Status,
            EnrolledStudents = c.ClassMembers?.Count(cm => cm.Status == BusinessObject.Enum.ClassMemberStatus.ACTIVE) ?? 0
        }).ToList();
    }

    public async Task<TeacherScheduleClassDto?> GetClassByIdAsync(long classId, long teacherId)
    {
        var c = await repo.GetClassByIdAsync(classId, teacherId);
        if (c == null) return null;
        
        return new TeacherScheduleClassDto
        {
            Id = c.Id,
            Name = c.Name,
            Subject = c.Subject,
            Status = c.Status,
            EnrolledStudents = c.ClassMembers?.Count(cm => cm.Status == BusinessObject.Enum.ClassMemberStatus.ACTIVE) ?? 0
        };
    }

    public async Task<List<TeacherScheduleDto>> GetSchedulesByClassAsync(long classId)
    {
        var schedules = await repo.GetSchedulesByClassAsync(classId);
        return schedules.Select(s => new TeacherScheduleDto
        {
            Id = s.Id,
            ClassId = s.ClassId,
            Title = s.Title,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            Type = s.Type,
            Status = s.Status,
            IsTrial = s.IsTrial,
            AttendanceCount = s.Attendances?.Count(a => a.IsPresent) ?? 0,
            TotalStudents = s.Attendances?.Count() ?? 0
        }).ToList();
    }

    public async Task<TeacherScheduleDto?> GetScheduleByIdAsync(long scheduleId)
    {
        var s = await repo.GetScheduleByIdAsync(scheduleId);
        if (s == null) return null;

        return new TeacherScheduleDto
        {
            Id = s.Id,
            ClassId = s.ClassId,
            Title = s.Title,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            Type = s.Type,
            Status = s.Status,
            IsTrial = s.IsTrial,
            AttendanceCount = s.Attendances?.Count(a => a.IsPresent) ?? 0,
            TotalStudents = s.Attendances?.Count() ?? 0
        };
    }

    public async Task<ApiResponse<object>> CreateScheduleAsync(long teacherId, CreateScheduleDto dto)
    {
        // verify teacher owns class
        var c = await repo.GetClassByIdAsync(dto.ClassId, teacherId);
        if (c == null) return ApiResponse<object>.Fail("Class not found or access denied.");

        if (dto.StartTime >= dto.EndTime) return ApiResponse<object>.Fail("End time must be after start time.");

        var schedule = new Schedule
        {
            ClassId = dto.ClassId,
            Title = dto.Title,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Type = dto.Type,
            Status = dto.Status,
            IsTrial = dto.IsTrial,
            CreatedAt = DateTime.UtcNow
        };

        await repo.CreateScheduleAsync(schedule);

        return ApiResponse<object>.Ok(schedule.Id, "Schedule created successfully");
    }

    public async Task<ApiResponse<object>> UpdateScheduleAsync(long teacherId, long scheduleId, UpdateScheduleDto dto)
    {
        var schedule = await repo.GetScheduleByIdAsync(scheduleId);
        if (schedule == null) return ApiResponse<object>.Fail("Schedule not found.");

        var c = await repo.GetClassByIdAsync(schedule.ClassId, teacherId);
        if (c == null) return ApiResponse<object>.Fail("Access denied.");

        if (dto.StartTime >= dto.EndTime) return ApiResponse<object>.Fail("End time must be after start time.");

        schedule.Title = dto.Title;
        schedule.StartTime = dto.StartTime;
        schedule.EndTime = dto.EndTime;
        schedule.Type = dto.Type;
        schedule.Status = dto.Status;
        schedule.IsTrial = dto.IsTrial;

        await repo.UpdateScheduleAsync(schedule);

        return ApiResponse<object>.Ok(schedule.Id, "Schedule updated successfully");
    }

    public async Task<ApiResponse<object>> DeleteScheduleAsync(long teacherId, long scheduleId)
    {
        var schedule = await repo.GetScheduleByIdAsync(scheduleId);
        if (schedule == null) return ApiResponse<object>.Fail("Schedule not found.");

        var c = await repo.GetClassByIdAsync(schedule.ClassId, teacherId);
        if (c == null) return ApiResponse<object>.Fail("Access denied.");

        await repo.DeleteScheduleAsync(schedule);

        return ApiResponse<object>.Ok(null, "Schedule deleted successfully");
    }

    public async Task<List<StudentAttendanceDto>> GetAttendanceForScheduleAsync(long scheduleId)
    {
        var schedule = await repo.GetScheduleByIdAsync(scheduleId);
        if (schedule == null) return new List<StudentAttendanceDto>();

        var studentsInClass = await repo.GetClassStudentsAsync(schedule.ClassId);
        var existingAttendances = await repo.GetAttendancesByScheduleAsync(scheduleId);

        var result = new List<StudentAttendanceDto>();

        foreach (var student in studentsInClass)
        {
            var record = existingAttendances.FirstOrDefault(a => a.StudentId == student.Id);
            result.Add(new StudentAttendanceDto
            {
                StudentId = student.Id,
                StudentName = student.StudentProfile?.FullName ?? "Unknown",
                StudentEmail = student.Email,
                AttendanceId = record?.Id ?? 0,
                IsPresent = record?.IsPresent ?? false,
                Notes = record?.Notes
            });
        }

        return result.OrderBy(a => a.StudentName).ToList();
    }

    public async Task<ApiResponse<object>> SaveAttendanceAsync(long teacherId, long scheduleId, List<StudentAttendanceDto> dtos)
    {
        var schedule = await repo.GetScheduleByIdAsync(scheduleId);
        if (schedule == null) return ApiResponse<object>.Fail("Schedule not found.");

        var c = await repo.GetClassByIdAsync(schedule.ClassId, teacherId);
        if (c == null) return ApiResponse<object>.Fail("Access denied.");

        var existingAttendances = await repo.GetAttendancesByScheduleAsync(scheduleId);
        var toAdd = new List<Attendance>();
        var toUpdate = new List<Attendance>();

        foreach (var dto in dtos)
        {
            if (dto.AttendanceId == 0)
            {
                toAdd.Add(new Attendance
                {
                    ScheduleId = scheduleId,
                    StudentId = dto.StudentId,
                    IsPresent = dto.IsPresent,
                    Notes = dto.Notes,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                var existing = existingAttendances.FirstOrDefault(a => a.Id == dto.AttendanceId);
                if (existing != null)
                {
                    existing.IsPresent = dto.IsPresent;
                    existing.Notes = dto.Notes;
                    existing.UpdatedAt = DateTime.UtcNow;
                    toUpdate.Add(existing);
                }
            }
        }

        await repo.SaveAttendancesAsync(toAdd, toUpdate);

        return ApiResponse<object>.Ok(null, "Attendance saved successfully");
    }
}
