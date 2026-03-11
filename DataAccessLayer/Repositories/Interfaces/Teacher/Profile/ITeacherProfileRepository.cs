using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces.Teacher.Profile;

public interface ITeacherProfileRepository
{
    Task<TeacherProfile?> GetByUserIdAsync(long userId);
    Task UpdateAsync(TeacherProfile profile);
}

