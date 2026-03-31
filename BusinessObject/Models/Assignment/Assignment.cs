using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("assignments")]
public class Assignment : SoftDeletableEntity
{
    public long ClassId { get; set; }

    public long TeacherId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    public AssignmentStatus Status { get; set; } = AssignmentStatus.DRAFT;

    public DateTime? DueDate { get; set; }

    [MaxLength(1000)]
    public string? FileUrl { get; set; }

    // Navigation properties
    [ForeignKey("ClassId")]
    public Class Class { get; set; } = null!;

    [ForeignKey("TeacherId")]
    public User Teacher { get; set; } = null!;

    public ICollection<AssignmentQuestion> Questions { get; set; } = [];
    public ICollection<Submission> Submissions { get; set; } = [];
}
