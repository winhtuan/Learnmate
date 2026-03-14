using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Settings;

public class JwtSettings
{
    [Required]
    public string SecretKey { get; init; } = string.Empty;

    [Required]
    public string Issuer { get; init; } = "learnmate";

    [Required]
    public string Audience { get; init; } = "learnmate";
    public int ExpiryMinutes { get; init; } = 60;
}
