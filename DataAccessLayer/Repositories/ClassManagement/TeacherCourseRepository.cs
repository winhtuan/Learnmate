using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class TeacherCourseRepository : ITeacherCourseRepository
{
    private readonly AppDbContext db;

    public TeacherCourseRepository(AppDbContext context)
    {
        db = context;
    }

    public async Task<Class> CreateAsync(Class cls)
    {
        db.Classes.Add(cls);
        await db.SaveChangesAsync();
        return cls;
    }

    public async Task<Class?> GetTeacherClassDetailAsync(long classId, long teacherId, CancellationToken ct = default)
    {
        return await db.Classes
            .AsNoTracking()
            .Where(c => c.Id == classId && c.TeacherId == teacherId && c.DeletedAt == null)
            .Include(c => c.ClassMembers.Where(m => m.Status == ClassMemberStatus.ACTIVE))
                .ThenInclude(m => m.Student).ThenInclude(u => u.StudentProfile)
            .Include(c => c.Assignments.Where(a => a.DeletedAt == null).OrderByDescending(a => a.CreatedAt))
                .ThenInclude(a => a.Submissions.Where(s => s.DeletedAt == null))
            .Include(c => c.Schedules.Where(s => s.Status != ScheduleStatus.CANCELLED).OrderByDescending(s => s.StartTime).Take(50))
            .Include(c => c.Materials.Where(m => m.Status == MaterialStatus.ACTIVE).OrderByDescending(m => m.CreatedAt))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<Class>> GetByTeacherIdAsync(long teacherId, CancellationToken ct = default)
    {
        var nowUtc = DateTime.UtcNow;
        return await db.Classes
            .AsNoTracking()
            .Where(c => c.TeacherId == teacherId && c.DeletedAt == null)
            .Include(c => c.ClassMembers.Where(m => m.Status == ClassMemberStatus.ACTIVE))
            .Include(c => c.Schedules.Where(s => s.Status != ScheduleStatus.CANCELLED && s.StartTime > nowUtc).OrderBy(s => s.StartTime).Take(1))
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<Material> AddMaterialAsync(Material material, CancellationToken ct = default)
    {
        material.CreatedAt = DateTime.UtcNow;
        material.UpdatedAt = DateTime.UtcNow;
        db.Materials.Add(material);
        await db.SaveChangesAsync(ct);
        return material;
    }

    public async Task DeleteMaterialAsync(long materialId, long classId, CancellationToken ct = default)
    {
        var material = await db.Materials
            .FirstOrDefaultAsync(m => m.Id == materialId && m.ClassId == classId, ct);
        if (material is not null)
        {
            material.Status = MaterialStatus.HIDDEN;
            material.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);
        }
    }
}
