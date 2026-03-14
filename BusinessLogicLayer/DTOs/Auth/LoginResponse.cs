namespace BusinessLogicLayer.DTOs.Auth;

public record LoginResponse(
    long   UserId,
    string AccessToken,
    string TokenType,
    int    ExpiresIn,
    string Email,
    string Role,
    string? AvatarUrl
);

