using System.ComponentModel.DataAnnotations;
using BusinessObject.Enum;

namespace BusinessLogicLayer.DTOs.Teacher.Assignments;

public class UpdateAssignmentDto
{
    [Required]
    public long ClassId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public AssignmentStatus Status { get; set; }

    public List<UpdateAssignmentQuestionDto> Questions { get; set; } = new();
}

public class UpdateAssignmentQuestionDto
{
    public long? Id { get; set; } // null = new question

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    public QuestionType Type { get; set; } = QuestionType.MULTIPLE_CHOICE;

    public decimal Points { get; set; } = 1;

    public List<UpdateAssignmentOptionDto> Options { get; set; } = new();
}

public class UpdateAssignmentOptionDto
{
    public long? Id { get; set; } // null = new option

    [Required]
    public string Content { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }
}
