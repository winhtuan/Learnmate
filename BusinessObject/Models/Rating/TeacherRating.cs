using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("teacher_ratings")]
public class TeacherRating : AuditableEntity
{
    public long StudentId { get; set; }

    public long TeacherId { get; set; }

    public long ClassId { get; set; }

    [Required]
    public decimal Rating { get; set; }

    public string? Comment { get; set; }

    // Navigation properties
    [ForeignKey("StudentId")]
    public User Student { get; set; } = null!;

    [ForeignKey("TeacherId")]
    public User Teacher { get; set; } = null!;

    [ForeignKey("ClassId")]
    public Class Class { get; set; } = null!;
}
