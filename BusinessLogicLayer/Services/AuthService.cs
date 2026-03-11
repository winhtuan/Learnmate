using System.IdentityModel.Tokens.Jwt;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Auth;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services;

public class AuthService(
    IUserRepository userRepo,
    IOtpVerificationRepository otpRepo,
    IStudentProfileRepository studentProfileRepo,
    IJwtService jwtService,
    IOtpService otpService,
    IEmailService emailService,
    ILogger<AuthService> logger) : IAuthService
{
    private const int MaxOtpAttempts = 5;

    public async Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterRequest request)
    {
        // Validate role
        if (!Enum.TryParse<UserRole>(request.Role.ToUpper(), out var role) || role == UserRole.ADMIN)
            return ApiResponse<RegisterResponse>.Fail("Invalid role. Allowed values: STUDENT, TEACHER.");

        var existingUser = await userRepo.GetByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            // If already verified, reject
            if (existingUser.IsActive)
                return ApiResponse<RegisterResponse>.Fail("An account with this email already exists.");

            // Unverified — resend OTP
            await SendOtpAsync(existingUser);
            return ApiResponse<RegisterResponse>.Ok(
                new RegisterResponse(existingUser.Id, existingUser.Email,
                    "Account already registered. A new verification code has been sent."));
        }

        var user = new User
        {
            Email = request.Email.ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = role,
            IsActive = false  // remains inactive until email verified
        };

        user = await userRepo.CreateAsync(user);

        if (role == UserRole.STUDENT)
        {
            await studentProfileRepo.CreateAsync(new StudentProfile
            {
                UserId = user.Id,
                FullName = request.FullName ?? request.Email,
                DateOfBirth = request.DateOfBirth
            });
        }

        await SendOtpAsync(user);

        return ApiResponse<RegisterResponse>.Ok(
            new RegisterResponse(user.Id, user.Email,
                "Registration successful. Please check your email for the verification code."));
    }

    public async Task<ApiResponse<object?>> VerifyOtpAsync(VerifyOtpRequest request)
    {
        var user = await userRepo.GetByEmailAsync(request.Email);
        if (user is null)
            return ApiResponse<object?>.Fail("Invalid email or verification code.");

        if (user.IsActive)
            return ApiResponse<object?>.Fail("Account is already verified.");

        var otp = await otpRepo.GetLatestActiveByUserIdAsync(user.Id);

        if (otp is null)
            return ApiResponse<object?>.Fail("No active verification code found. Please request a new one.");

        if (otp.AttemptCount >= MaxOtpAttempts)
        {
            otp.IsUsed = true;
            await otpRepo.UpdateAsync(otp);
            return ApiResponse<object?>.Fail("Too many attempts. Please request a new verification code.");
        }

        if (otpService.IsExpired(otp.ExpiredAt))
        {
            otp.IsUsed = true;
            await otpRepo.UpdateAsync(otp);
            return ApiResponse<object?>.Fail("Verification code has expired. Please request a new one.");
        }

        otp.AttemptCount++;

        if (otp.Code != request.Code)
        {
            await otpRepo.UpdateAsync(otp);
            int remaining = MaxOtpAttempts - otp.AttemptCount;
            return ApiResponse<object?>.Fail($"Invalid verification code. {remaining} attempt(s) remaining.");
        }

        // Mark OTP used and activate user
        otp.IsUsed = true;
        await otpRepo.UpdateAsync(otp);

        user.IsActive = true;
        await userRepo.UpdateAsync(user);

        return ApiResponse<object?>.Ok(null, "Email verified successfully. You can now sign in.");
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        logger.LogInformation("[Login] Attempt for email: {Email}", request.Email);

        var user = await userRepo.GetByEmailAsync(request.Email);

        if (user is null)
        {
            logger.LogWarning("[Login] User not found: {Email}", request.Email);
            return ApiResponse<LoginResponse>.Fail("Invalid email or password.");
        }

        logger.LogInformation("[Login] User found: id={Id}, isActive={IsActive}, hashPrefix={HashPrefix}",
            user.Id, user.IsActive, user.PasswordHash[..10]);

        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        logger.LogInformation("[Login] BCrypt.Verify result: {Result}", passwordValid);

        if (!passwordValid)
            return ApiResponse<LoginResponse>.Fail("Invalid email or password.");

        if (!user.IsActive)
            return ApiResponse<LoginResponse>.Fail(
                "Your account is not verified. Please check your email for the verification code.");

        var token = jwtService.GenerateToken(user.Id, user.Email, user.Role.ToString());

        var expiryMinutes = 60; // default; JwtService reads from config

        return ApiResponse<LoginResponse>.Ok(new LoginResponse(
            UserId:      user.Id,
            AccessToken: token,
            TokenType:   "Bearer",
            ExpiresIn:   expiryMinutes * 60,
            Email:       user.Email,
            Role:        user.Role.ToString(),
            AvatarUrl:   user.AvatarUrl));
    }

    public Task<ApiResponse<object?>> LogoutAsync(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            jwtService.BlacklistToken(token, jwt.ValidTo);
        }
        catch
        {
            // Token might be malformed — still return success (idempotent logout)
        }

        return Task.FromResult(ApiResponse<object?>.Ok(null, "Logged out successfully."));
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private async Task SendOtpAsync(User user)
    {
        await otpRepo.InvalidateAllForUserAsync(user.Id);

        var otp = new OtpVerification
        {
            UserId = user.Id,
            Code = otpService.GenerateCode(),
            ExpiredAt = otpService.GetExpiryTime()
        };

        await otpRepo.CreateAsync(otp);
        await emailService.SendOtpEmailAsync(user.Email, otp.Code);
    }
}
