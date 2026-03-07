using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("assignment_options")]
public class AssignmentOption
{
    [Key]
    public long Id { get; set; }

    public long QuestionId { get; set; }

    [Required]
    public string Content { get; set; } = null!;

    public bool IsCorrect { get; set; } = false;

    public int Order { get; set; }

    // Navigation properties
    [ForeignKey("QuestionId")]
    public AssignmentQuestion Question { get; set; } = null!;

    public ICollection<SubmissionAnswerOption> SubmissionAnswerOptions { get; set; } = [];
}
