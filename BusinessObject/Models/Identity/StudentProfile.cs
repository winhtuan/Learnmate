using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("student_profiles")]
public class StudentProfile : AuditableEntity
{
    public long UserId { get; set; }

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    [MaxLength(20)]
    public string? GradeLevel { get; set; }

    [MaxLength(20)]
    public string? ParentContact { get; set; }

    public int StudyStreakDays { get; set; }

    // Navigation properties
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}
