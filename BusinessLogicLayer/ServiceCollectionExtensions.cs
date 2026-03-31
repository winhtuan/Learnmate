using BusinessLogicLayer.Services;
using BusinessLogicLayer.Services.Dashboard;
using BusinessLogicLayer.Services.Interfaces;
using BusinessLogicLayer.Services.Interfaces.Dashboard;
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
        services.AddScoped<IStudentClassMemberRepository, StudentClassMemberRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IStudentClassRepository, StudentClassRepository>();
        services.AddScoped<ITeacherAssignmentRepository, TeacherAssignmentRepository>();
        services.AddScoped<ITeacherCourseRepository, TeacherCourseRepository>();
        services.AddScoped<ITeacherProfileRepository, TeacherProfileRepository>();
        services.AddScoped<ITeacherDocumentRepository, TeacherDocumentRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<ITutorBookingRepository, TutorBookingRepository>();
        services.AddScoped<IConversationRepository, DataAccessLayer.Repositories.System.ConversationRepository>();
        services.AddScoped<DataAccessLayer.Repositories.Interfaces.ClassManagement.IClassMessageRepository, DataAccessLayer.Repositories.ClassManagement.ClassMessageRepository>();
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
        services.AddScoped<ITeacherDashboardService, TeacherDashboardService>();
        services.AddScoped<IStudentClassService, StudentClassService>();
        services.AddScoped<BusinessLogicLayer.Services.Interfaces.Teacher.Schedules.ITeacherScheduleService, BusinessLogicLayer.Services.Teacher.Schedules.TeacherScheduleService>();
        services.AddScoped<BusinessLogicLayer.Services.Interfaces.Teacher.Schedules.IVoiceScheduleAnalyzer, BusinessLogicLayer.Services.Teacher.Schedules.VoiceScheduleAnalyzer>();
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<IClassManagementService, ClassManagementService>();
        services.AddScoped<ITeacherComplianceService, TeacherComplianceService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<ITutorService, TutorService>();
        services.AddScoped<IMessagingService, BusinessLogicLayer.Services.System.MessagingService>();
        services.AddScoped<BusinessLogicLayer.Services.Interfaces.ClassManagement.IClassMessageService, BusinessLogicLayer.Services.ClassManagement.ClassMessageService>();

        // Token blacklist cache
        services.AddMemoryCache();

        return services;
    }
}
