using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.UserManagement;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services;

public class UserManagementService(AppDbContext db) : IUserManagementService
{
    public async Task<IEnumerable<UserRowDto>> GetUsersAsync(string roleFilter = "", string statusFilter = "", string searchQuery = "")
    {
        var query = db.Users.AsQueryable();

        // Include profiles for Names
        query = query.Include(u => u.StudentProfile).Include(u => u.TeacherProfile);

        if (!string.IsNullOrEmpty(roleFilter))
        {
            var filterParam = roleFilter.ToUpper() == "INSTRUCTOR" ? "TEACHER" : roleFilter.ToUpper();
            if (Enum.TryParse<UserRole>(filterParam, out var roleEnum))
                query = query.Where(u => u.Role == roleEnum);
        }

        if (!string.IsNullOrEmpty(statusFilter))
        {
            if (statusFilter.Equals("Active", StringComparison.OrdinalIgnoreCase))
                query = query.Where(u => u.IsActive);
            else if (statusFilter.Equals("Blocked", StringComparison.OrdinalIgnoreCase))
                query = query.Where(u => !u.IsActive);
        }

        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            query = query.Where(u => u.Email.ToLower().Contains(searchQuery) 
                                  || (u.StudentProfile != null && u.StudentProfile.FullName.ToLower().Contains(searchQuery))
                                  || (u.TeacherProfile != null && u.TeacherProfile.FullName.ToLower().Contains(searchQuery))
                                  || u.Id.ToString().Contains(searchQuery));
        }

        var users = await query.OrderByDescending(u => u.CreatedAt).ToListAsync();

        return users.Select(u => new UserRowDto
        {
            Id = u.Id,
            Email = u.Email,
            Role = u.Role == UserRole.TEACHER ? "Instructor" : (u.Role == UserRole.STUDENT ? "Student" : "Admin"),
            Status = u.IsActive ? "Active" : "Blocked",
            LastLogin = "N/A", // This could be updated if we track last login
            AvatarUrl = u.AvatarUrl ?? "",
            Name = u.Role == UserRole.STUDENT ? (u.StudentProfile?.FullName ?? u.Email)
                 : u.Role == UserRole.TEACHER ? (u.TeacherProfile?.FullName ?? u.Email)
                 : "Administrator"
        });
    }

    public async Task<ApiResponse<object?>> CreateUserAsync(NewUserRequestDto request)
    {
        var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());
        if (existingUser != null)
            return ApiResponse<object?>.Fail("An account with this email already exists.");

        var roleStr = request.Role.ToUpper() == "INSTRUCTOR" ? "TEACHER" : request.Role.ToUpper();
        if (!Enum.TryParse<UserRole>(roleStr, out var role))
            return ApiResponse<object?>.Fail("Invalid role specified.");

        var user = new User
        {
            Email = request.Email.ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.TempPassword),
            Role = role,
            IsActive = true
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        if (role == UserRole.STUDENT)
        {
            db.StudentProfiles.Add(new StudentProfile
            {
                UserId = user.Id,
                FullName = request.Name,
            });
        }
        else if (role == UserRole.TEACHER)
        {
            db.TeacherProfiles.Add(new TeacherProfile
            {
                UserId = user.Id,
                FullName = request.Name,
                Subjects = "N/A",
                HourlyRate = 50.0m
            });
        }

        await db.SaveChangesAsync();
        return ApiResponse<object?>.Ok(null, "User created successfully.");
    }

    public async Task<ApiResponse<object?>> ChangeUserStatusAsync(long userId, bool isActive, string reason, string notes = "")
    {
        var user = await db.Users.FindAsync(userId);
        if (user == null) return ApiResponse<object?>.Fail("User not found.");

        user.IsActive = isActive;
        // Optionally store reason and notes in an Audit log table if it existed

        db.Users.Update(user);
        await db.SaveChangesAsync();

        return ApiResponse<object?>.Ok(null, isActive ? "User unblocked successfully." : "User blocked successfully.");
    }

    public async Task<ApiResponse<object?>> DeleteUserAsync(long userId)
    {
        var user = await db.Users.FindAsync(userId);
        if (user == null) return ApiResponse<object?>.Fail("User not found.");

        user.DeletedAt = DateTime.UtcNow;
        db.Users.Update(user);
        await db.SaveChangesAsync();

        return ApiResponse<object?>.Ok(null, "User deleted successfully.");
    }
}
