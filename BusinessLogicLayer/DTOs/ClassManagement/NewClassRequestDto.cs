using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTOs.ClassManagement;

public class NewClassRequestDto
{
    [Required]
    public long TeacherId { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Subject { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Range(1, 1000)]
    public int MaxStudents { get; set; }
}
