using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces.Teacher.Assignments;

public interface ITeacherAssignmentRepository
{
    Task<Assignment> CreateAsync(Assignment assignment);
    Task<List<Assignment>> GetByTeacherIdAsync(long teacherId);
    Task<Assignment?> GetByIdWithDetailsAsync(long assignmentId);
    Task UpdateAsync(Assignment assignment);
}
