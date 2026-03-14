using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Teacher.Assignments;

namespace BusinessLogicLayer.Services.Interfaces;

public interface ITeacherAssignmentService
{
    Task<ApiResponse<object>> CreateAssignmentAsync(long teacherId, CreateAssignmentDto dto);
    Task<List<AssignmentListItemDto>> GetAssignmentsByTeacherAsync(long teacherId);
    Task<AssignmentDetailDto?> GetAssignmentByIdAsync(long assignmentId);
    Task<ApiResponse<object>> UpdateAssignmentAsync(long assignmentId, UpdateAssignmentDto dto);
}
