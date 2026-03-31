using System.Text;
using BusinessLogicLayer;
using BusinessLogicLayer.Services.Interfaces;
using BusinessLogicLayer.Settings;
using DataAccessLayer;
using LearnmateSolution.Components;
using LearnmateSolution.AppState;
using LearnmateSolution.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace LearnmateSolution;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ── .env ──────────────────────────────────────────────────────────────
        var envFile = Path.Combine(Directory.GetCurrentDirectory(), ".env");
        if (File.Exists(envFile))
        {
            foreach (var rawLine in File.ReadAllLines(envFile))
            {
                var line = rawLine.Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith('#')) continue;
                var eq = line.IndexOf('=');
                if (eq < 0) continue;
                Environment.SetEnvironmentVariable(line[..eq].Trim(), line[(eq + 1)..].Trim());
            }
        }

        if (Environment.GetEnvironmentVariable("EMAIL_SENDER") is { } emailSender)
            builder.Configuration["EmailSettings:SenderEmail"] = emailSender;
        if (Environment.GetEnvironmentVariable("EMAIL_PASSWORD") is { } emailPassword)
            builder.Configuration["EmailSettings:Password"] = emailPassword;

        if (Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME") is { } cloudName)
            builder.Configuration["Cloudinary:CloudName"] = cloudName;
        if (Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY") is { } cloudApiKey)
            builder.Configuration["Cloudinary:ApiKey"] = cloudApiKey;
        if (Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET") is { } cloudApiSecret)
            builder.Configuration["Cloudinary:ApiSecret"] = cloudApiSecret;

        if (Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME") is { } cloudinaryName)
            builder.Configuration["Cloudinary:CloudName"] = cloudinaryName;
        if (Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY") is { } cloudinaryKey)
            builder.Configuration["Cloudinary:ApiKey"] = cloudinaryKey;
        if (Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET") is { } cloudinarySecret)
            builder.Configuration["Cloudinary:ApiSecret"] = cloudinarySecret;

        // ── Database ──────────────────────────────────────────────────────────
        builder.Services.AddDataAccessLayer(builder.Configuration);

        // ── Business Logic + Repositories ────────────────────────────────────
        builder.Services.AddBusinessLogicLayer();

        // ── JWT Settings (IOptions<JwtSettings>) ─────────────────────────────
        builder.Services.AddOptions<JwtSettings>()
            .BindConfiguration("Jwt")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // ── JWT Authentication ────────────────────────────────────────────────
        var jwtSecret = builder.Configuration["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("Jwt:SecretKey is not configured.");

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    ClockSkew = TimeSpan.Zero
                };

                // Allow SignalR to pass JWT via query string (?access_token=...)
                options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Query["access_token"];
                        var path  = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/hubs"))
                            context.Token = token;
                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddAuthorization();

        // ── Web API controllers ───────────────────────────────────────────────
        builder.Services.AddControllers();

        // ── SignalR ───────────────────────────────────────────────────────────
        builder.Services.AddSignalR();

        // ── HttpClient (for Blazor components calling own API) ────────────────
        builder.Services.AddHttpClient("learnmate", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // ── Cloudinary file storage ───────────────────────────────────────────
        builder.Services.AddOptions<CloudinarySettings>()
            .BindConfiguration("Cloudinary")
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.Services.AddSingleton<IFileStorageService, CloudinaryFileStorageService>();

        // ── VNPay Settings ────────────────────────────────────────────────────
        builder.Services.AddOptions<VnPaySettings>()
            .BindConfiguration("VnPay");

        // ── Booking Expiry Background Service ─────────────────────────────────
        builder.Services.AddHostedService<BookingExpiryBackgroundService>();

        // ── Auth session (per Blazor circuit) ────────────────────────────────
        builder.Services.AddScoped<UserSessionService>();
        builder.Services.AddScoped<ToastService>();

        // ── Blazor Server ─────────────────────────────────────────────────────
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents(options =>
            {
                options.DetailedErrors = builder.Environment.IsDevelopment();
            });

        var app = builder.Build();

        // ── HTTP Pipeline ─────────────────────────────────────────────────────
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAntiforgery();

        // API routes
        app.MapControllers();

        // SignalR hubs
        app.MapHub<LearnmateSolution.Hubs.ChatHub>("/hubs/chat");

        // Blazor routes
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
