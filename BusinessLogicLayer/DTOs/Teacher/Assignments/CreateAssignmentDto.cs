using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTOs.Teacher.Assignments;

public class CreateAssignmentDto
{
    [Required]
    public long ClassId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public List<CreateAssignmentQuestionDto> Questions { get; set; } = new();
}
