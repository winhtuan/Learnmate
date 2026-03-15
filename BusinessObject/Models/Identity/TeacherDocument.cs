using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("teacher_documents")]
public class TeacherDocument : AuditableEntity
{
    public long TeacherProfileId { get; set; }

    [Required]
    [MaxLength(255)]
    public string DocumentName { get; set; } = null!;

    [Required]
    [MaxLength(500)]
    public string FileUrl { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string FileType { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = "Other";

    public long FileSize { get; set; }

    [ForeignKey("TeacherProfileId")]
    public TeacherProfile TeacherProfile { get; set; } = null!;
}
