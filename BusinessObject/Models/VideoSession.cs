using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("video_sessions")]
public class VideoSession : AuditableEntity
{
    public long ScheduleId { get; set; }

    [Required]
    public VideoProvider Provider { get; set; }

    [Required]
    [MaxLength(500)]
    public string MeetingUrl { get; set; } = null!;

    [MaxLength(100)]
    public string? MeetingId { get; set; }

    [Required]
    public VideoSessionStatus Status { get; set; } = VideoSessionStatus.WAITING;

    public DateTime? StartedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    // Navigation properties
    [ForeignKey("ScheduleId")]
    public Schedule Schedule { get; set; } = null!;
}
