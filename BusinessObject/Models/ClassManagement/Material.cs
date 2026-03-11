using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("materials")]
public class Material : AuditableEntity
{
    public long ClassId { get; set; }

    public long UploadedBy { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    [MaxLength(500)]
    public string FileUrl { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string FileType { get; set; } = null!;

    public long? FileSizeBytes { get; set; }

    [Required]
    public MaterialStatus Status { get; set; } = MaterialStatus.ACTIVE;

    // Navigation properties
    [ForeignKey("ClassId")]
    public Class Class { get; set; } = null!;

    [ForeignKey("UploadedBy")]
    public User Uploader { get; set; } = null!;
}
