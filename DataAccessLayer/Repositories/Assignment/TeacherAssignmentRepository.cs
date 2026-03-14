using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class TeacherAssignmentRepository(AppDbContext db) : ITeacherAssignmentRepository
{
    public async Task<Assignment> CreateAsync(Assignment assignment)
    {
        db.Assignments.Add(assignment);
        await db.SaveChangesAsync();
        return assignment;
    }

    public async Task<List<Assignment>> GetByTeacherIdAsync(long teacherId)
    {
        return await db
            .Assignments.Where(a => a.TeacherId == teacherId && a.DeletedAt == null)
            .Include(a => a.Class)
            .Include(a => a.Questions)
            .Include(a => a.Submissions)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Assignment?> GetByIdWithDetailsAsync(long assignmentId)
    {
        return await db
            .Assignments.Where(a => a.DeletedAt == null)
            .Include(a => a.Class)
            .Include(a => a.Questions.OrderBy(q => q.Order))
            .ThenInclude(q => q.Options.OrderBy(o => o.Order))
            .FirstOrDefaultAsync(a => a.Id == assignmentId);
    }

    public async Task UpdateAsync(Assignment assignment)
    {
        // Remove old options first, then questions
        var existingQuestions = await db
            .AssignmentQuestions.Where(q => q.AssignmentId == assignment.Id)
            .Include(q => q.Options)
            .ToListAsync();

        foreach (var q in existingQuestions)
        {
            db.AssignmentOptions.RemoveRange(q.Options);
        }
        db.AssignmentQuestions.RemoveRange(existingQuestions);

        // Now update the assignment (EF will add the new questions/options)
        db.Assignments.Update(assignment);
        await db.SaveChangesAsync();
    }
}
