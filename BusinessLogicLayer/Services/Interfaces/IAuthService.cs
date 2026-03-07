using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Auth;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterRequest request);
    Task<ApiResponse<object?>> VerifyOtpAsync(VerifyOtpRequest request);
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse<object?>> LogoutAsync(string token);
}
