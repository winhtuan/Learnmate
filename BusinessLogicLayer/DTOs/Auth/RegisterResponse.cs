namespace BusinessLogicLayer.DTOs.Auth;

public record RegisterResponse(
    long UserId,
    string Email,
    string Message
);
