using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("submission_answer_options")]
public class SubmissionAnswerOption
{
    [Key]
    public long Id { get; set; }

    public long SubmissionAnswerId { get; set; }

    public long OptionId { get; set; }

    // Navigation properties
    [ForeignKey("SubmissionAnswerId")]
    public SubmissionAnswer SubmissionAnswer { get; set; } = null!;

    [ForeignKey("OptionId")]
    public AssignmentOption Option { get; set; } = null!;
}
