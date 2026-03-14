using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class StudentProfileRepository(AppDbContext db) : IStudentProfileRepository
{
    public async Task<StudentProfile> CreateAsync(StudentProfile profile)
    {
        db.StudentProfiles.Add(profile);
        await db.SaveChangesAsync();
        return profile;
    }

    public Task<StudentProfile?> GetByUserIdAsync(long userId) =>
        db
            .StudentProfiles.AsNoTracking()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);
}
