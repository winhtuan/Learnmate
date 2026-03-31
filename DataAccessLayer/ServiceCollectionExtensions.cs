using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccessLayer(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Allow DateTime without strict UTC — simplifies development
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured."
            );

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention()
        );

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITeacherProfileRepository, TeacherProfileRepository>();
        services.AddScoped<ITeacherDocumentRepository, TeacherDocumentRepository>();
        services.AddScoped<IStudentProfileRepository, StudentProfileRepository>();

        return services;
    }
}
