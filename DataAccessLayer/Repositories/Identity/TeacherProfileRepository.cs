using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class TeacherProfileRepository(AppDbContext db) : ITeacherProfileRepository
{
    public Task<TeacherProfile?> GetByUserIdAsync(long userId) =>
        db.TeacherProfiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);

    public async Task AddAsync(TeacherProfile profile)
    {
        await db.TeacherProfiles.AddAsync(profile);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(TeacherProfile profile)
    {
        db.TeacherProfiles.Update(profile);
        await db.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<TeacherProfile>> GetAllTeachersAsync(
        string? subjectFilter = null,
        decimal? maxRate = null,
        CancellationToken ct = default)
    {
        var query = db.TeacherProfiles
            .AsNoTracking()
            .Include(p => p.User)
            .Where(p => p.User.IsActive && p.User.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(subjectFilter))
            query = query.Where(p => EF.Functions.ILike(p.Subjects, $"%{subjectFilter}%"));

        if (maxRate.HasValue)
            query = query.Where(p => p.HourlyRate <= maxRate.Value);

        return await query
            .OrderByDescending(p => p.RatingAvg)
            .ThenByDescending(p => p.TotalRatingCount)
            .ToListAsync(ct);
    }
}
