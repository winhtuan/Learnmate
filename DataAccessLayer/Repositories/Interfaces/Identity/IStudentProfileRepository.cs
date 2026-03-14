using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IStudentProfileRepository
{
    Task<StudentProfile> CreateAsync(StudentProfile profile);
    Task<StudentProfile?> GetByUserIdAsync(long userId);
}
