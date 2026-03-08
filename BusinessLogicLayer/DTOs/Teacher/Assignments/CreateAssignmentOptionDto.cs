using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTOs.Teacher.Assignments;

public class CreateAssignmentOptionDto
{
    [Required]
    public string Content { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }
}
