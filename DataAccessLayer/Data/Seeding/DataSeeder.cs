using BusinessObject.Enum;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data.Seeding;

/// <summary>
/// Seeds initial data for development and testing.
///
/// Passwords are BCrypt hashes. To regenerate:
///   BCrypt.Net.BCrypt.HashPassword("YourPassword", workFactor: 12)
///
/// Seeded accounts:
///   admin@learnmate.vn   / Admin@123
///   teacher@learnmate.vn / Teacher@123
///   student1@gmail.com   / Student@123
///   student2@gmail.com   / Student@123
/// </summary>
public static class DataSeeder
{
    private static readonly DateTime Now = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    // BCrypt hashes (workFactor: 12) — regenerate if needed
    private const string AdminHash    = "$2a$12$LXTRCOBrEnOJuJcMl.ixPuqfqexImjFMmhWK4A4hcqxaW59WkS5K2";
    private const string TeacherHash  = "$2a$12$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi";
    private const string StudentHash  = "$2a$12$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi";

    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedUsers(modelBuilder);
        SeedProfiles(modelBuilder);
        SeedClasses(modelBuilder);
        SeedClassMembers(modelBuilder);
    }

    // ─────────────────────────────────────────────────────────────────────────
    private static void SeedUsers(ModelBuilder m)
    {
        m.Entity<User>().HasData(
            new User
            {
                Id          = 1,
                Email       = "admin@learnmate.vn",
                PasswordHash = AdminHash,
                Role        = UserRole.ADMIN,
                IsActive    = true,
                CreatedAt   = Now,
                UpdatedAt   = Now,
            },
            new User
            {
                Id          = 2,
                Email       = "teacher@learnmate.vn",
                PasswordHash = TeacherHash,
                Role        = UserRole.TEACHER,
                IsActive    = true,
                CreatedAt   = Now,
                UpdatedAt   = Now,
            },
            new User
            {
                Id          = 3,
                Email       = "student1@gmail.com",
                PasswordHash = StudentHash,
                Role        = UserRole.STUDENT,
                IsActive    = true,
                CreatedAt   = Now,
                UpdatedAt   = Now,
            },
            new User
            {
                Id          = 4,
                Email       = "student2@gmail.com",
                PasswordHash = StudentHash,
                Role        = UserRole.STUDENT,
                IsActive    = true,
                CreatedAt   = Now,
                UpdatedAt   = Now,
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
                Id        = 1,
                UserId    = 3,
                FullName  = "Tran Thi B",
                GradeLevel = "12",
                CreatedAt = Now,
                UpdatedAt = Now,
            },
            new StudentProfile
            {
                Id        = 2,
                UserId    = 4,
                FullName  = "Le Van C",
                GradeLevel = "11",
                CreatedAt = Now,
                UpdatedAt = Now,
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
            },
            new ClassMember
            {
                Id        = 2,
                ClassId   = 1,
                StudentId = 4,
                Status    = ClassMemberStatus.ACTIVE,
                JoinedAt  = Now,
            }
        );
    }
}
