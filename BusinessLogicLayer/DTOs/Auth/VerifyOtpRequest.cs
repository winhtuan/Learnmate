using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTOs.Auth;

public record VerifyOtpRequest(
    [Required, EmailAddress] string Email,
    [Required, MinLength(6), MaxLength(6)] string Code
);
