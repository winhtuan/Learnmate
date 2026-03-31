using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;

namespace BusinessLogicLayer.Services;

public class TeacherComplianceService(
    ITeacherProfileRepository profileRepo,
    ITeacherDocumentRepository documentRepo,
    AppDbContext db) : ITeacherComplianceService
{
    public async Task<ApiResponse<object>> SubmitApplicationAsync(long userId, List<TeacherDocumentRequestDto> documents)
    {
        var profile = await profileRepo.GetByUserIdAsync(userId);
        if (profile == null) 
        {
            // Final fallback
            profile = new TeacherProfile { UserId = userId, FullName = "Unknown", Subjects = "", Status = ComplianceStatus.NONE };
            await profileRepo.AddAsync(profile);
        }

        // Clear existing documents if any (or we could append)
        var existingDocs = await documentRepo.GetByTeacherProfileIdAsync(profile.Id);
        foreach (var doc in existingDocs)
        {
            await documentRepo.DeleteAsync(doc.Id);
        }

        foreach (var docDto in documents)
        {
            await documentRepo.AddAsync(new TeacherDocument
            {
                TeacherProfileId = profile.Id,
                DocumentName = docDto.Name,
                FileUrl = docDto.FileUrl,
                FileType = docDto.FileType,
                Category = docDto.Category,
                FileSize = docDto.FileSize
            });
        }

        profile.Status = ComplianceStatus.PENDING;
        await profileRepo.UpdateAsync(profile);

        return ApiResponse<object>.Ok(null, "Application submitted successfully.");
    }

    public async Task<IEnumerable<TeacherApplicationDto>> GetAllApplicationsAsync()
    {
        var profiles = await db.TeacherProfiles
            .Include(p => p.User)
            .Include(p => p.Documents)
            .Where(p => p.Status != ComplianceStatus.NONE)
            .OrderByDescending(p => p.UpdatedAt)
            .ToListAsync();

        return profiles.Select(p => MapToDto(p));
    }

    public async Task<TeacherApplicationDto?> GetApplicationByIdAsync(long userId)
    {
        var profile = await db.TeacherProfiles
            .Include(p => p.User)
            .Include(p => p.Documents)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        return profile == null ? null : MapToDto(profile);
    }

    public async Task<ApiResponse<object>> ReviewApplicationAsync(long userId, ComplianceStatus status, string? notes)
    {
        var profile = await profileRepo.GetByUserIdAsync(userId);
        if (profile == null) return ApiResponse<object>.Fail("Tutor profile not found.");

        if (profile.Status == ComplianceStatus.APPROVED || profile.Status == ComplianceStatus.REJECTED)
        {
            return ApiResponse<object>.Fail($"Application is already {profile.Status} and cannot be modified.");
        }

        profile.Status = status;
        profile.AdminNotes = notes;
        
        if (status == ComplianceStatus.APPROVED)
        {
            profile.VerifiedAt = DateTime.UtcNow;
        }

        await profileRepo.UpdateAsync(profile);

        return ApiResponse<object>.Ok(null, $"Application {status.ToString().ToLower()} successfully.");
    }

    private static TeacherApplicationDto MapToDto(TeacherProfile p)
    {
        return new TeacherApplicationDto
        {
            UserId = p.UserId,
            FullName = p.FullName,
            Email = p.User?.Email ?? "",
            AvatarUrl = p.AvatarUrl,
            Subjects = p.Subjects,
            Bio = p.Bio,
            HourlyRate = p.HourlyRate,
            LanguagesSpoken = p.LanguagesSpoken,
            YearsOfExperience = p.YearsOfExperience,
            TeachingPhilosophy = p.TeachingPhilosophy,
            Status = p.Status,
            SubmittedAt = p.UpdatedAt,
            AdminNotes = p.AdminNotes,
            Documents = p.Documents.Select(d => new TeacherDocumentDto
            {
                Id = d.Id,
                Name = d.DocumentName,
                FileUrl = d.FileUrl,
                FileType = d.FileType,
                Category = d.Category,
                FileSize = d.FileSize
            }).ToList()
        };
    }
}
