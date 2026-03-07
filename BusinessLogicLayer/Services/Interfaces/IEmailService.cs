namespace BusinessLogicLayer.Services.Interfaces;

public interface IEmailService
{
    Task SendOtpEmailAsync(string toEmail, string otpCode);
}
