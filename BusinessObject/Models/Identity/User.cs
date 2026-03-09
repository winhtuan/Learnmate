using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("users")]
public class User : SoftDeletableEntity
{
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public UserRole Role { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(500)]
    public string? AvatarUrl { get; set; }

    // Navigation properties
    public TeacherProfile? TeacherProfile { get; set; }
    public StudentProfile? StudentProfile { get; set; }

    public ICollection<Class> Classes { get; set; } = [];
    public ICollection<ClassMember> ClassMembers { get; set; } = [];
    public ICollection<Assignment> Assignments { get; set; } = [];
    public ICollection<Submission> Submissions { get; set; } = [];
    public ICollection<Material> Materials { get; set; } = [];
    public ICollection<Payment> Payments { get; set; } = [];
    public ICollection<Invoice> Invoices { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];

    [InverseProperty("Student")]
    public ICollection<TeacherRating> RatingsGiven { get; set; } = [];

    [InverseProperty("Teacher")]
    public ICollection<TeacherRating> RatingsReceived { get; set; } = [];
}
