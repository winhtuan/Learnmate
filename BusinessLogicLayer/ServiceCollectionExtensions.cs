using BusinessLogicLayer.Services;
using BusinessLogicLayer.Services.Interfaces;
using BusinessLogicLayer.Services.Interfaces.Teacher.Assignments;
using BusinessLogicLayer.Services.Interfaces.Teacher.Courses;
using BusinessLogicLayer.Services.Interfaces.Teacher.Profile;
using BusinessLogicLayer.Services.Teacher.Assignments;
using BusinessLogicLayer.Services.Teacher.Courses;
using BusinessLogicLayer.Services.Teacher.Profile;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Repositories.Interfaces.Teacher.Assignments;
using DataAccessLayer.Repositories.Interfaces.Teacher.Courses;
using DataAccessLayer.Repositories.Interfaces.Teacher.Profile;
using DataAccessLayer.Repositories.Teacher.Assignments;
using DataAccessLayer.Repositories.Teacher.Courses;
using DataAccessLayer.Repositories.Teacher.Profile;
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
        services.AddScoped<DataAccessLayer.Repositories.Interfaces.Teacher.Assignments.ITeacherAssignmentRepository, DataAccessLayer.Repositories.Teacher.Assignments.TeacherAssignmentRepository>();
        services.AddScoped<DataAccessLayer.Repositories.Interfaces.Teacher.Schedules.ITeacherScheduleRepository, DataAccessLayer.Repositories.Teacher.Schedules.TeacherScheduleRepository>();

                // Services
                services.AddScoped<IAuthService, AuthService>();
                services.AddScoped<IEmailService, EmailService>();
                services.AddScoped<IOtpService, OtpService>();
                services.AddScoped<ITeacherAssignmentService, TeacherAssignmentService>();
                services.AddScoped<ITeacherCourseService, TeacherCourseService>();
                services.AddScoped<ITeacherProfileService, TeacherProfileService>();
                services.AddSingleton<IJwtService, JwtService>();
                services.AddScoped<IDashboardService, DashboardService>();
                services.AddScoped<IStudentDashboardService, StudentDashboardService>();
                services.AddScoped<IClassService, ClassService>();
                services.AddScoped<IUserManagementService, UserManagementService>();
                services.AddScoped<IClassManagementService, ClassManagementService>();
                services.AddScoped<IReportService, ReportService>();

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<BusinessLogicLayer.Services.Interfaces.Teacher.Assignments.ITeacherAssignmentService, BusinessLogicLayer.Services.Teacher.Assignments.TeacherAssignmentService>();
        services.AddScoped<BusinessLogicLayer.Services.Interfaces.Teacher.Schedules.ITeacherScheduleService, BusinessLogicLayer.Services.Teacher.Schedules.TeacherScheduleService>();
        services.AddScoped<BusinessLogicLayer.Services.Interfaces.Teacher.Schedules.IVoiceScheduleAnalyzer, BusinessLogicLayer.Services.Teacher.Schedules.VoiceScheduleAnalyzer>();
        services.AddSingleton<IJwtService, JwtService>();
       
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IStudentDashboardService, StudentDashboardService>();

                // Token blacklist cache
                services.AddMemoryCache();

                return services;
        }
}
