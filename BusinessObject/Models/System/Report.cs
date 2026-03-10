using System.ComponentModel.DataAnnotations;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models.System;

public class Report : AuditableEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public ReportCategory Category { get; set; }

    [Required]
    public ReportFormat Format { get; set; }

    [Required]
    public ReportStatus Status { get; set; }

    public DateTime RequestedOn { get; set; } = DateTime.UtcNow;

    public string? FileUrl { get; set; }
}
