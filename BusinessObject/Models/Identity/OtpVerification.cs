using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("otp_verifications")]
public class OtpVerification : AuditableEntity
{
    public long UserId { get; set; }

    [Required]
    [MaxLength(6)]
    public string Code { get; set; } = null!;

    public DateTime ExpiredAt { get; set; }

    public bool IsUsed { get; set; } = false;

    public int AttemptCount { get; set; } = 0;

    // Navigation
    public User User { get; set; } = null!;
}
