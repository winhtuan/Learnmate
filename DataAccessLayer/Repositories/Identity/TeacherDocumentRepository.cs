using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class TeacherDocumentRepository(AppDbContext db) : ITeacherDocumentRepository
{
    public async Task<IEnumerable<TeacherDocument>> GetByTeacherProfileIdAsync(long profileId) =>
        await db.TeacherDocuments
            .Where(d => d.TeacherProfileId == profileId)
            .ToListAsync();

    public async Task AddAsync(TeacherDocument document)
    {
        db.TeacherDocuments.Add(document);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var doc = await db.TeacherDocuments.FindAsync(id);
        if (doc != null)
        {
            db.TeacherDocuments.Remove(doc);
            await db.SaveChangesAsync();
        }
    }
}
