using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("classes")]
public class Class : SoftDeletableEntity
{
    public long TeacherId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    [MaxLength(100)]
    public string Subject { get; set; } = null!;

    [Required]
    public ClassStatus Status { get; set; } = ClassStatus.ACTIVE;

    public int MaxStudents { get; set; } = 30;

    [MaxLength(500)]
    public string? ThumbnailUrl { get; set; } = "https://placehold.co/400?text=Course";

    // Navigation properties
    [ForeignKey("TeacherId")]
    public User Teacher { get; set; } = null!;

    public ICollection<ClassMember> ClassMembers { get; set; } = [];
    public ICollection<Schedule> Schedules { get; set; } = [];
    public ICollection<Assignment> Assignments { get; set; } = [];
    public ICollection<Material> Materials { get; set; } = [];
    public ICollection<Payment> Payments { get; set; } = [];
    public ICollection<Invoice> Invoices { get; set; } = [];
    public ICollection<TeacherRating> TeacherRatings { get; set; } = [];
}
