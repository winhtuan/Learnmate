using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("schedules")]
public class Schedule : AuditableEntity
{
    public long ClassId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    [Required]
    public ScheduleType Type { get; set; }

    [Required]
    public ScheduleStatus Status { get; set; } = ScheduleStatus.SCHEDULED;

    public bool IsTrial { get; set; } = false;

    // Navigation properties
    [ForeignKey("ClassId")]
    public Class Class { get; set; } = null!;

    public VideoSession? VideoSession { get; set; }
    public ICollection<Attendance> Attendances { get; set; } = [];
}
