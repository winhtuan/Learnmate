using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("assignment_questions")]
public class AssignmentQuestion : AuditableEntity
{
    public long AssignmentId { get; set; }

    [Required]
    public string Content { get; set; } = null!;

    [Required]
    public QuestionType Type { get; set; }

    public int Order { get; set; }

    [Required]
    public decimal Points { get; set; }

    // Navigation properties
    [ForeignKey("AssignmentId")]
    public Assignment Assignment { get; set; } = null!;

    public ICollection<AssignmentOption> Options { get; set; } = [];
    public ICollection<SubmissionAnswer> SubmissionAnswers { get; set; } = [];
}
