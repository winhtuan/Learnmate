using BusinessLogicLayer.DTOs.Teacher.Classes;

namespace BusinessLogicLayer.Services.Interfaces;

public interface ITeacherCourseService
{
    Task<long> CreateClassAsync(long teacherId, CreateClassDto dto);
    Task<TeacherClassDetailDto?> GetTeacherClassDetailAsync(long classId, long teacherId, CancellationToken ct = default);
    Task<List<TeacherClassListItemDto>> GetTeacherClassesAsync(long teacherId, CancellationToken ct = default);
}
