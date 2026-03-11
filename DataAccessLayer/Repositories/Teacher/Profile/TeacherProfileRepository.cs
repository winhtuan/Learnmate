using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces.Teacher.Profile;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Teacher.Profile;

public class TeacherProfileRepository(AppDbContext db) : ITeacherProfileRepository
{
    public Task<TeacherProfile?> GetByUserIdAsync(long userId) =>
        db.TeacherProfiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);

    public async Task UpdateAsync(TeacherProfile profile)
    {
        db.TeacherProfiles.Update(profile);
        await db.SaveChangesAsync();
    }
}

