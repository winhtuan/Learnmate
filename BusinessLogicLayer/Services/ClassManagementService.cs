using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.ClassManagement;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services;

public class ClassManagementService(AppDbContext db) : IClassManagementService
{
    public async Task<IEnumerable<ClassRowDto>> GetClassesAsync(string statusFilter = "", string searchQuery = "")
    {
        var query = db.Classes
            .Include(c => c.Teacher)
            .ThenInclude(t => t.TeacherProfile)
            .Include(c => c.ClassMembers)
            .AsQueryable();

        if (!string.IsNullOrEmpty(statusFilter))
        {
            if (Enum.TryParse<ClassStatus>(statusFilter, true, out var statusEnum))
                query = query.Where(c => c.Status == statusEnum);
        }

        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(searchQuery) 
                                  || c.Subject.ToLower().Contains(searchQuery)
                                  || (c.Teacher != null && c.Teacher.TeacherProfile != null && c.Teacher.TeacherProfile.FullName.ToLower().Contains(searchQuery))
                                  || c.Id.ToString().Contains(searchQuery));
        }

        var classes = await query.OrderByDescending(c => c.CreatedAt).ToListAsync();

        return classes.Select(c => new ClassRowDto
        {
            Id = c.Id,
            Name = c.Name,
            Subject = c.Subject,
            TeacherName = c.Teacher?.TeacherProfile?.FullName ?? c.Teacher?.Email ?? "Unknown",
            Status = c.Status.ToString(),
            EnrolledCount = c.ClassMembers?.Count ?? 0,
            MaxStudents = c.MaxStudents,
            CreatedAt = c.CreatedAt.ToString("MMM dd, yyyy")
        });
    }

    public async Task<ApiResponse<object?>> CreateClassAsync(NewClassRequestDto request)
    {
        var teacher = await db.Users.FirstOrDefaultAsync(u => u.Id == request.TeacherId && u.Role == UserRole.TEACHER);
        if (teacher == null)
            return ApiResponse<object?>.Fail("Teacher not found.");

        var newClass = new Class
        {
            TeacherId = request.TeacherId,
            Name = request.Name,
            Subject = request.Subject,
            Description = request.Description,
            Status = ClassStatus.ACTIVE
        };

        db.Classes.Add(newClass);
        await db.SaveChangesAsync();

        return ApiResponse<object?>.Ok(null, "Class created successfully.");
    }

    public async Task<ApiResponse<object?>> ChangeClassStatusAsync(long classId, string newStatus)
    {
        var cls = await db.Classes.FindAsync(classId);
        if (cls == null) return ApiResponse<object?>.Fail("Class not found.");

        if (Enum.TryParse<ClassStatus>(newStatus, true, out var statusEnum))
        {
            cls.Status = statusEnum;
            db.Classes.Update(cls);
            await db.SaveChangesAsync();
            return ApiResponse<object?>.Ok(null, $"Class status updated to {statusEnum}.");
        }

        return ApiResponse<object?>.Fail("Invalid status.");
    }

    public async Task<ApiResponse<object?>> DeleteClassAsync(long classId)
    {
        var cls = await db.Classes.FindAsync(classId);
        if (cls == null) return ApiResponse<object?>.Fail("Class not found.");

        // Instead of soft delete let's just physically delete or update status based on schema. 
        // The DB has a DeletedAt?
        cls.DeletedAt = DateTime.UtcNow;
        db.Classes.Update(cls);
        await db.SaveChangesAsync();

        return ApiResponse<object?>.Ok(null, "Class deleted successfully.");
    }

    public async Task<IEnumerable<TeacherDropdownDto>> GetAvailableTeachersAsync()
    {
        return await db.Users
            .Include(u => u.TeacherProfile)
            .Where(u => u.Role == UserRole.TEACHER && u.IsActive && u.DeletedAt == null)
            .Select(u => new TeacherDropdownDto
            {
                Id = u.Id,
                Name = u.TeacherProfile != null ? u.TeacherProfile.FullName : u.Email
            })
            .ToListAsync();
    }
}
