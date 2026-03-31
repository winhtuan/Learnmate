using BusinessObject.Enum;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data.Seeding;

public static class DataSeeder
{
    private static readonly DateTime Now = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private const string Hash123 = "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka";

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Email = "admin@learnmate.vn", PasswordHash = Hash123, Role = UserRole.ADMIN, IsActive = true, CreatedAt = Now, UpdatedAt = Now },
            new User { Id = 2, Email = "teacher@learnmate.vn", PasswordHash = Hash123, Role = UserRole.TEACHER, IsActive = true, CreatedAt = Now, UpdatedAt = Now },
            new User { Id = 3, Email = "student@learnmate.vn", PasswordHash = Hash123, Role = UserRole.STUDENT, IsActive = true, CreatedAt = Now, UpdatedAt = Now }
        );

        modelBuilder.Entity<TeacherProfile>().HasData(
            new TeacherProfile { Id = 1, UserId = 2, FullName = "Nguyen Van A", Subjects = "Toán", HourlyRate = 200000m, CreatedAt = Now, UpdatedAt = Now }
        );

        modelBuilder.Entity<StudentProfile>().HasData(
            new StudentProfile { Id = 1, UserId = 3, FullName = "Nguyen Minh Tuan", GradeLevel = "12", CreatedAt = Now, UpdatedAt = Now }
        );

        modelBuilder.Entity<Class>().HasData(
            new Class { Id = 1, TeacherId = 2, Name = "Lớp Toán Test", Subject = "Toán", Status = ClassStatus.ACTIVE, MaxStudents = 20, MeetingLink = "https://meet.google.com/abc-test-meet", CreatedAt = Now, UpdatedAt = Now }
        );
    }
}
