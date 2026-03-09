using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTOs.Auth;

public record RegisterRequest(
    [Required, EmailAddress, MaxLength(255)] string Email,
    [Required, MinLength(8), MaxLength(128)] string Password,
    [Required] string Role,         // "STUDENT" or "TEACHER"
    [MaxLength(200)] string? FullName = null,
    DateOnly? DateOfBirth = null
);
