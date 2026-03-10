using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("submissions")]
public class Submission : SoftDeletableEntity
{
    public long AssignmentId { get; set; }

    public long StudentId { get; set; }

    [Required]
    public SubmissionStatus Status { get; set; } = SubmissionStatus.DRAFT;

    public decimal? Score { get; set; }

    public DateTime? SubmittedAt { get; set; }

    [MaxLength(1000)]
    public string? FileUrl { get; set; }

    // Navigation properties
    [ForeignKey("AssignmentId")]
    public Assignment Assignment { get; set; } = null!;

    [ForeignKey("StudentId")]
    public User Student { get; set; } = null!;

    public ICollection<SubmissionAnswer> Answers { get; set; } = [];
    public Feedback? Feedback { get; set; }
}
