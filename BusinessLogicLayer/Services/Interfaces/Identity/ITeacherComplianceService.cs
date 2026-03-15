using BusinessLogicLayer.DTOs;
using BusinessObject.Enum;

namespace BusinessLogicLayer.Services.Interfaces;

public interface ITeacherComplianceService
{
    Task<ApiResponse<object>> SubmitApplicationAsync(long userId, List<TeacherDocumentRequestDto> documents);
    Task<IEnumerable<TeacherApplicationDto>> GetAllApplicationsAsync();
    Task<TeacherApplicationDto?> GetApplicationByIdAsync(long userId);
    Task<ApiResponse<object>> ReviewApplicationAsync(long userId, ComplianceStatus status, string? notes);
}

public class TeacherDocumentRequestDto
{
    public string Name { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public string FileType { get; set; } = null!;
    public string Category { get; set; } = "Other";
    public long FileSize { get; set; }
}

public class TeacherApplicationDto
{
    public long UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public string Subjects { get; set; } = null!;
    public string? Bio { get; set; }
    public decimal HourlyRate { get; set; }
    public string? LanguagesSpoken { get; set; }
    public int? YearsOfExperience { get; set; }
    public string? TeachingPhilosophy { get; set; }
    public ComplianceStatus Status { get; set; }
    public DateTime SubmittedAt { get; set; }
    public string? AdminNotes { get; set; }
    public List<TeacherDocumentDto> Documents { get; set; } = [];
}

public class TeacherDocumentDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public string FileType { get; set; } = null!;
    public string Category { get; set; } = "Other";
    public long FileSize { get; set; }
}
