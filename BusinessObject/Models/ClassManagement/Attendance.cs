using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("attendances")]
public class Attendance : AuditableEntity
{
    public long ScheduleId { get; set; }

    public long StudentId { get; set; }

    public bool IsPresent { get; set; } = false;

    public string? Notes { get; set; }

    // Navigation properties
    [ForeignKey("ScheduleId")]
    public Schedule Schedule { get; set; } = null!;

    [ForeignKey("StudentId")]
    public User Student { get; set; } = null!;
}
