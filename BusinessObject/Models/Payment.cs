using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("payments")]
public class Payment : SoftDeletableEntity
{
    public long StudentId { get; set; }

    public long ClassId { get; set; }

    public long? InvoiceId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public PaymentType Type { get; set; }

    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.PENDING;

    public DateTime? PaidAt { get; set; }

    // Navigation properties
    [ForeignKey("StudentId")]
    public User Student { get; set; } = null!;

    [ForeignKey("ClassId")]
    public Class Class { get; set; } = null!;

    [ForeignKey("InvoiceId")]
    public Invoice? Invoice { get; set; }
}
