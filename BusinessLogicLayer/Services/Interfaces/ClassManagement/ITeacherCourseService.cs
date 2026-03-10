using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Teacher.Courses;

namespace BusinessLogicLayer.Services.Interfaces;

public interface ITeacherCourseService
{
    Task<ApiResponse<object>> CreateCourseAsync(int teacherId, CreateCourseDto dto);
    // Add other course-specific methods (e.g., GetMyCourses)
}
