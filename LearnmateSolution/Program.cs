using System.Text;
using BusinessLogicLayer;
using BusinessLogicLayer.Settings;
using DataAccessLayer;
using LearnmateSolution.Components;
using LearnmateSolution.AppState;
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
            });

        builder.Services.AddAuthorization();

        // ── Web API controllers ───────────────────────────────────────────────
        builder.Services.AddControllers();

        // ── HttpClient (for Blazor components calling own API) ────────────────
        builder.Services.AddHttpClient("learnmate", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // ── Auth session (per Blazor circuit) ────────────────────────────────
        builder.Services.AddScoped<UserSessionService>();

        // ── Blazor Server ─────────────────────────────────────────────────────
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

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

        // Blazor routes
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
