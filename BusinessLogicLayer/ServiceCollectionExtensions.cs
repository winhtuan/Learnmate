using BusinessLogicLayer.Services;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOtpVerificationRepository, OtpVerificationRepository>();
        services.AddScoped<IStudentProfileRepository, StudentProfileRepository>();
        services.AddScoped<IClassMemberRepository, ClassMemberRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IOtpService, OtpService>();
        services.AddSingleton<IJwtService, JwtService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IStudentDashboardService, StudentDashboardService>();

        // Token blacklist cache
        services.AddMemoryCache();

        return services;
    }
}
