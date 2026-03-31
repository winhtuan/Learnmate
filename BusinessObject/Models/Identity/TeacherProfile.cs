using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("teacher_profiles")]
public class TeacherProfile : AuditableEntity
{
    public long UserId { get; set; }

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = null!;

    [MaxLength(500)]
    public string? AvatarUrl { get; set; }

    public string? Bio { get; set; }

    [Required]
    [MaxLength(500)]
    public string Subjects { get; set; } = null!;

    [Required]
    public decimal HourlyRate { get; set; }

    [MaxLength(500)]
    public string? LanguagesSpoken { get; set; }

    public int? YearsOfExperience { get; set; }

    public string? TeachingPhilosophy { get; set; }

    public decimal RatingAvg { get; set; } = 0;

    public int TotalRatingCount { get; set; } = 0;

    [MaxLength(100)]
    public string? BankAccount { get; set; }

    public ComplianceStatus Status { get; set; } = ComplianceStatus.PENDING;

    public string? AdminNotes { get; set; }

    public DateTime? VerifiedAt { get; set; }

    // Navigation properties
    public ICollection<TeacherDocument> Documents { get; set; } = [];

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}
