using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer.Repositories;

public class StudentProfileRepository(AppDbContext db) : IStudentProfileRepository
{
    public async Task<StudentProfile> CreateAsync(StudentProfile profile)
    {
        db.StudentProfiles.Add(profile);
        await db.SaveChangesAsync();
        return profile;
    }
}
