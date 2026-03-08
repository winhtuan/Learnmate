namespace BusinessLogicLayer.DTOs.Auth;

public record LoginResponse(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    string Email,
    string Role,
    string? AvatarUrl
);
