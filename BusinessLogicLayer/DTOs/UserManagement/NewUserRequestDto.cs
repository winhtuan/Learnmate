using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTOs.UserManagement;

public class NewUserRequestDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty;

    [Required, MinLength(8)]
    public string TempPassword { get; set; } = string.Empty;
}
