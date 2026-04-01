using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Teacher.Schedules;

namespace BusinessLogicLayer.Services.Interfaces.Teacher.Schedules;

public interface ITeacherScheduleService
{
    Task<List<TeacherScheduleClassDto>> GetTeacherClassesAsync(long teacherId);
    Task<TeacherScheduleClassDto?> GetClassByIdAsync(long classId, long teacherId);
    Task<List<TeacherScheduleDto>> GetSchedulesByClassAsync(long classId);
    Task<TeacherScheduleDto?> GetScheduleByIdAsync(long scheduleId);
    
    Task<ApiResponse<object>> CreateScheduleAsync(long teacherId, CreateScheduleDto dto);
    Task<ApiResponse<object>> BulkCreateSchedulesAsync(long teacherId, List<CreateScheduleDto> dtos);
    Task<ApiResponse<object>> UpdateScheduleAsync(long teacherId, long scheduleId, UpdateScheduleDto dto);
    Task<ApiResponse<object>> DeleteScheduleAsync(long teacherId, long scheduleId);

    Task<List<StudentAttendanceDto>> GetAttendanceForScheduleAsync(long scheduleId);
    Task<ApiResponse<object>> SaveAttendanceAsync(long teacherId, long scheduleId, List<StudentAttendanceDto> dtos);
}
