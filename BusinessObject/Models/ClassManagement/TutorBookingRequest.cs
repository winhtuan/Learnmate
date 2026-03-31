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

    /// <summary>Teacher từ chối với lý do này, hoặc hệ thống cancel</summary>
    [MaxLength(1000)]
    public string? CancelReason { get; set; }

    [Required]
    public BookingRequestStatus Status { get; set; } = BookingRequestStatus.PENDING;

    /// <summary>Class mà student sẽ join sau khi thanh toán thành công</summary>
    public long? ClassId { get; set; }

    /// <summary>Hạn chót student phải thanh toán (24h sau khi teacher duyệt)</summary>
    public DateTime? PaymentDeadline { get; set; }

    // Sau khi teacher ACCEPTED, hệ thống tạo Class 1-1 và gán vào đây
    public long? ResultClassId { get; set; }

    [ForeignKey("StudentId")]
    public User Student { get; set; } = null!;

    [ForeignKey("TeacherId")]
    public User Teacher { get; set; } = null!;

    [ForeignKey("ResultClassId")]
    public Class? ResultClass { get; set; }
}
