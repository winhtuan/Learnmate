using System.ComponentModel.DataAnnotations;
using BusinessObject.Enum;

namespace BusinessLogicLayer.DTOs.Teacher.Assignments;

public class CreateAssignmentQuestionDto
{
    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    public QuestionType Type { get; set; } = QuestionType.MULTIPLE_CHOICE;

    public decimal Points { get; set; } = 1;

    public List<CreateAssignmentOptionDto> Options { get; set; } = new();
}
