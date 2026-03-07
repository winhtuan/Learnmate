using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTOs.Auth;

public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password
);
