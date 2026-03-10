using BusinessLogicLayer.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace BusinessLogicLayer.Services;

public class EmailService(IConfiguration config) : IEmailService
{
    public async Task SendOtpEmailAsync(string toEmail, string otpCode)
    {
        var senderEmail =
            config["EmailSettings:SenderEmail"]
            ?? throw new InvalidOperationException("EmailSettings:SenderEmail is not configured.");
        var senderName = config["EmailSettings:SenderName"] ?? "LearnMate";
        var password =
            config["EmailSettings:Password"]
            ?? throw new InvalidOperationException("EmailSettings:Password is not configured.");
        var host = config["EmailSettings:Host"] ?? "smtp.gmail.com";
        var port = config.GetValue<int>("EmailSettings:Port", 587);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderName, senderEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = "Your LearnMate verification code";

        message.Body = new TextPart("html")
        {
            Text = $"""
                <div style="font-family:sans-serif;max-width:480px;margin:0 auto">
                  <h2 style="color:#0f172a">Verify your email</h2>
                  <p>Use the code below to complete your registration. It expires in 5 minutes.</p>
                  <div style="font-size:2rem;font-weight:700;letter-spacing:.5rem;
                              background:#f8fafc;border:2px solid #e2e8f0;border-radius:8px;
                              padding:1rem 2rem;text-align:center;color:#0f172a">
                    {otpCode}
                  </div>
                  <p style="color:#64748b;font-size:.875rem;margin-top:1.5rem">
                    If you did not create an account, you can safely ignore this email.
                  </p>
                </div>
                """,
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(senderEmail, password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
