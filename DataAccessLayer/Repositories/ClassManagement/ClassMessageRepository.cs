using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces.ClassManagement;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.ClassManagement;

public class ClassMessageRepository(AppDbContext db) : IClassMessageRepository
{
    public async Task<List<ClassMessage>> GetClassMessagesAsync(long classId, int skip, int take, CancellationToken ct)
    {
        return await db.ClassMessages
            .Include(m => m.Sender).ThenInclude(u => u.TeacherProfile)
            .Include(m => m.Sender).ThenInclude(u => u.StudentProfile)
            .Where(m => m.ClassId == classId)
            .OrderByDescending(m => m.CreatedAt) // newest first
            .Skip(skip)
            .Take(take)
            .ToListAsync(ct);
    }
    
    public async Task<bool> AddAsync(ClassMessage message, CancellationToken ct)
    {
        db.ClassMessages.Add(message);
        return await db.SaveChangesAsync(ct) > 0;
    }
}
