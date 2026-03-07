using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("submission_answers")]
public class SubmissionAnswer : AuditableEntity
{
    public long SubmissionId { get; set; }

    public long QuestionId { get; set; }

    public string? AnswerText { get; set; }

    // Navigation properties
    [ForeignKey("SubmissionId")]
    public Submission Submission { get; set; } = null!;

    [ForeignKey("QuestionId")]
    public AssignmentQuestion Question { get; set; } = null!;

    public ICollection<SubmissionAnswerOption> SelectedOptions { get; set; } = [];
}
