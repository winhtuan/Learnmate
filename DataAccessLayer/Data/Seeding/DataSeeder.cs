using BusinessObject.Enum;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data.Seeding;

/// <summary>
/// Seeds initial data for development and testing.
///
/// Passwords are BCrypt hashes (workFactor: 12). To regenerate:
///   BCrypt.Net.BCrypt.HashPassword("your_password", workFactor: 12)
///
/// Seeded accounts (all password: 123):
///   admin@learnmate.vn   / 123  (ADMIN)
///   teacher@learnmate.vn / 123  (TEACHER)
///   student@learnmate.vn / 123  (STUDENT)
/// </summary>
public static class DataSeeder
{
    private static readonly DateTime Now = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    // BCrypt hash for password "123" (workFactor: 12)
    private const string Hash123 = "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka";

    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedUsers(modelBuilder);
        SeedProfiles(modelBuilder);
        SeedClasses(modelBuilder);
        SeedClassMembers(modelBuilder);
        ClassSeed.Seed(modelBuilder);
    }

    // ─────────────────────────────────────────────────────────────────────────
    private static void SeedUsers(ModelBuilder m)
    {
        m.Entity<User>().HasData(
            new User
            {
                Id           = 1,
                Email        = "admin@learnmate.vn",
                PasswordHash = Hash123,
                Role         = UserRole.ADMIN,
                IsActive     = true,
                CreatedAt    = Now,
                UpdatedAt    = Now,
            },
            new User
            {
                Id           = 2,
                Email        = "teacher@learnmate.vn",
                PasswordHash = Hash123,
                Role         = UserRole.TEACHER,
                IsActive     = true,
                CreatedAt    = Now,
                UpdatedAt    = Now,
            },
            new User
            {
                Id           = 3,
                Email        = "student@learnmate.vn",
                PasswordHash = Hash123,
                Role         = UserRole.STUDENT,
                IsActive     = true,
                CreatedAt    = Now,
                UpdatedAt    = Now,
            }
        );
    }

    // ─────────────────────────────────────────────────────────────────────────
    private static void SeedProfiles(ModelBuilder m)
    {
        m.Entity<TeacherProfile>().HasData(
            new TeacherProfile
            {
                Id               = 1,
                UserId           = 2,
                FullName         = "Nguyen Van A",
                Bio              = "Giáo viên Toán với 5 năm kinh nghiệm.",
                Subjects         = "Toán, Vật Lý",
                HourlyRate       = 200_000m,
                RatingAvg        = 0m,
                TotalRatingCount = 0,
                CreatedAt        = Now,
                UpdatedAt        = Now,
            }
        );

        m.Entity<StudentProfile>().HasData(
            new StudentProfile
            {
                Id               = 1,
                UserId           = 3,
                FullName         = "Nguyen Minh Tuan",
                AvatarUrl        = "https://placehold.co/400?text=avatar",
                GradeLevel       = "12",
                StudyStreakDays   = 8,
                CreatedAt        = Now,
                UpdatedAt        = Now,
            }
        );
    }

    // ─────────────────────────────────────────────────────────────────────────
    private static void SeedClasses(ModelBuilder m)
    {
        m.Entity<Class>().HasData(
            new Class
            {
                Id          = 1,
                TeacherId   = 2,
                Name        = "Toán 12 — Luyện thi THPT",
                Description = "Ôn luyện toàn bộ chương trình Toán lớp 12.",
                Subject     = "Toán",
                Status      = ClassStatus.ACTIVE,
                MaxStudents = 20,
                CreatedAt   = Now,
                UpdatedAt   = Now,
            }
        );
    }

    // ─────────────────────────────────────────────────────────────────────────
    private static void SeedClassMembers(ModelBuilder m)
    {
        m.Entity<ClassMember>().HasData(
            new ClassMember
            {
                Id        = 1,
                ClassId   = 1,
                StudentId = 3,
                Status    = ClassMemberStatus.ACTIVE,
                JoinedAt  = Now,
            }
        );
    }
}
