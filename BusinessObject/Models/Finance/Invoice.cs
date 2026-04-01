using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("invoices")]
public class Invoice : SoftDeletableEntity
{
    public long TeacherId { get; set; }

    /// <summary>Student đăng ký lớp học — null nếu là invoice teacher payout.</summary>
    public long? StudentId { get; set; }

    public long ClassId { get; set; }

    [Required]
    public decimal TotalAmount { get; set; }

    [Required]
    public InvoiceStatus Status { get; set; } = InvoiceStatus.PENDING;

    public DateOnly PeriodStart { get; set; }

    public DateOnly PeriodEnd { get; set; }

    public DateTime IssuedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    // Navigation properties
    [ForeignKey("TeacherId")]
    public User Teacher { get; set; } = null!;

    [ForeignKey("ClassId")]
    public Class Class { get; set; } = null!;

    public ICollection<Payment> Payments { get; set; } = [];
}
