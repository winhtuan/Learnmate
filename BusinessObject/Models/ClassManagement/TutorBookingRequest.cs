using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("tutor_booking_requests")]
public class TutorBookingRequest : SoftDeletableEntity
{
    // StudentId → User với Role = STUDENT
    public long StudentId { get; set; }

    // TeacherId → User với Role = TEACHER
    public long TeacherId { get; set; }

    [Required]
    public DateTime RequestedStartTime { get; set; }

    [Required]
    public DateTime RequestedEndTime { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    [Required]
    public BookingRequestStatus Status { get; set; } = BookingRequestStatus.PENDING;

    // Sau khi teacher ACCEPTED, hệ thống tạo Class 1-1 và gán vào đây
    public long? ResultClassId { get; set; }

    [ForeignKey("StudentId")]
    public User Student { get; set; } = null!;

    [ForeignKey("TeacherId")]
    public User Teacher { get; set; } = null!;

    [ForeignKey("ResultClassId")]
    public Class? ResultClass { get; set; }
}
