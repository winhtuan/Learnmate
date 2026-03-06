using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("feedbacks")]
public class Feedback : AuditableEntity
{
    public long SubmissionId { get; set; }

    public string? Comment { get; set; }

    [Required]
    public decimal Score { get; set; }

    // Navigation properties
    [ForeignKey("SubmissionId")]
    public Submission Submission { get; set; } = null!;
}
