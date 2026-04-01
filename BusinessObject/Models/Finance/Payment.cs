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

    /// <summary>Booking Request liên kết (nếu thanh toán từ booking)</summary>
    public long? BookingId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public PaymentType Type { get; set; }

    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.PENDING;

    /// <summary>Mã giao dịch unique gửi sang VNPay (vnp_TxnRef)</summary>
    [MaxLength(100)]
    public string? VnpTxnRef { get; set; }

    /// <summary>Mã giao dịch VNPay trả về (vnp_TransactionNo)</summary>
    [MaxLength(100)]
    public string? VnpTransactionNo { get; set; }

    /// <summary>Phương thức thanh toán (enrollment flow).</summary>
    public PaymentMethod? Method { get; set; }

    /// <summary>Lý do thất bại (enrollment flow).</summary>
    [MaxLength(500)]
    public string? FailureReason { get; set; }

    /// <summary>Thông tin ngân hàng thanh toán</summary>
    [MaxLength(50)]
    public string? BankCode { get; set; }

    public DateTime? PaidAt { get; set; }

    /// <summary>Thời hạn thanh toán (used for expiry)</summary>
    public DateTime? ExpiredAt { get; set; }

    // Navigation properties
    [ForeignKey("StudentId")]
    public User Student { get; set; } = null!;

    [ForeignKey("ClassId")]
    public Class Class { get; set; } = null!;

    [ForeignKey("InvoiceId")]
    public Invoice? Invoice { get; set; }
}
