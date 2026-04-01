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
        SeedSchedules(modelBuilder);
        ClassSeed.Seed(modelBuilder);
        TutorClassSeed.Seed(modelBuilder);
    }

    // ─────────────────────────────────────────────────────────────────────────
    private static void SeedUsers(ModelBuilder m)
    {
        m.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Email = "admin@learnmate.vn",
                PasswordHash = Hash123,
                Role = UserRole.ADMIN,
                IsActive = true,
                AvatarUrl = "https://placehold.co/400?text=Admin",
                CreatedAt = Now,
                UpdatedAt = Now,
            },
            new User
            {
                Id = 2,
                Email = "teacher@learnmate.vn",
                PasswordHash = Hash123,
                Role = UserRole.TEACHER,
                IsActive = true,
                AvatarUrl = "https://placehold.co/400?text=Teacher",
                CreatedAt = Now,
                UpdatedAt = Now,
            },
            new User
            {
                Id = 3,
                Email = "student@learnmate.vn",
                PasswordHash = Hash123,
                Role = UserRole.STUDENT,
                IsActive = true,
                AvatarUrl = "https://placehold.co/400?text=Student",
                CreatedAt = Now,
                UpdatedAt = Now,
            },
            // ── 10 giáo viên bổ sung (id=4..13) ─────────────────────────────
            new User { Id = 4, Email = "tran.thi.mai@learnmate.vn", PasswordHash = Hash123, Role = UserRole.TEACHER, IsActive = true, AvatarUrl = "https://placehold.co/400/fce7f3/be185d?text=TM", CreatedAt = Now, UpdatedAt = Now },
            new User { Id = 5, Email = "le.van.duc@learnmate.vn", PasswordHash = Hash123, Role = UserRole.TEACHER, IsActive = true, AvatarUrl = "https://placehold.co/400/dcfce7/16a34a?text=LD", CreatedAt = Now, UpdatedAt = Now },
            new User { Id = 6, Email = "pham.thi.huong@learnmate.vn", PasswordHash = Hash123, Role = UserRole.TEACHER, IsActive = true, AvatarUrl = "https://placehold.co/400/fef9c3/ca8a04?text=PH", CreatedAt = Now, UpdatedAt = Now },
            new User { Id = 7, Email = "nguyen.quoc.bao@learnmate.vn", PasswordHash = Hash123, Role = UserRole.TEACHER, IsActive = true, AvatarUrl = "https://placehold.co/400/ede9fe/7c3aed?text=NB", CreatedAt = Now, UpdatedAt = Now },
            new User { Id = 8, Email = "hoang.thi.lan@learnmate.vn", PasswordHash = Hash123, Role = UserRole.TEACHER, IsActive = true, AvatarUrl = "https://placehold.co/400/ffedd5/ea580c?text=HL", CreatedAt = Now, UpdatedAt = Now },
            new User { Id = 9, Email = "vu.minh.khoa@learnmate.vn", PasswordHash = Hash123, Role = UserRole.TEACHER, IsActive = true, AvatarUrl = "https://placehold.co/400/cffafe/0891b2?text=VK", CreatedAt = Now, UpdatedAt = Now },
            new User { Id = 10, Email = "do.thi.thu@learnmate.vn", PasswordHash = Hash123, Role = UserRole.TEACHER, IsActive = true, AvatarUrl = "https://placehold.co/400/fce7f3/db2777?text=DT", CreatedAt = Now, UpdatedAt = Now },
            new User { Id = 11, Email = "bui.van.long@learnmate.vn", PasswordHash = Hash123, Role = UserRole.TEACHER, IsActive = true, AvatarUrl = "https://placehold.co/400/e0f2fe/0284c7?text=BL", CreatedAt = Now, UpdatedAt = Now },
            new User { Id = 12, Email = "nguyen.thi.bich@learnmate.vn", PasswordHash = Hash123, Role = UserRole.TEACHER, IsActive = true, AvatarUrl = "https://placehold.co/400/f1f5f9/475569?text=NB", CreatedAt = Now, UpdatedAt = Now },
            new User { Id = 13, Email = "trinh.van.nam@learnmate.vn", PasswordHash = Hash123, Role = UserRole.TEACHER, IsActive = true, AvatarUrl = "https://placehold.co/400/fef2f2/dc2626?text=TN", CreatedAt = Now, UpdatedAt = Now },
            // ── 2 học sinh bổ sung (id=14..15) ───────────────────────────────
            new User { Id = 14, Email = "student2@learnmate.vn", PasswordHash = Hash123, Role = UserRole.STUDENT, IsActive = true, AvatarUrl = "https://placehold.co/400/e0e7ff/4f46e5?text=S2", CreatedAt = Now, UpdatedAt = Now },
            new User { Id = 15, Email = "student3@learnmate.vn", PasswordHash = Hash123, Role = UserRole.STUDENT, IsActive = true, AvatarUrl = "https://placehold.co/400/ecfdf5/059669?text=S3", CreatedAt = Now, UpdatedAt = Now }
        );
    }

    // ─────────────────────────────────────────────────────────────────────────
    private static void SeedProfiles(ModelBuilder m)
    {
        m.Entity<TeacherProfile>().HasData(
            new TeacherProfile
            {
                Id = 1,
                UserId = 2,
                FullName = "Nguyen Van A",
                Bio = "Giáo viên Toán với 5 năm kinh nghiệm.",
                Subjects = "Toán, Vật Lý",
                HourlyRate = 35m,
                RatingAvg = 3.8m,
                TotalRatingCount = 5,
                CreatedAt = Now,
                UpdatedAt = Now,
            }
        );

        m.Entity<TeacherProfile>().HasData(
            new TeacherProfile { Id = 2, UserId = 4, FullName = "Tran Thi Mai", Bio = "Giáo viên Toán & Vật Lý với 7 năm kinh nghiệm luyện thi đại học. Hơn 200 học sinh đã đỗ các trường top.", Subjects = "Mathematics,Science", HourlyRate = 28m, RatingAvg = 4.5m, TotalRatingCount = 42, CreatedAt = Now, UpdatedAt = Now },
            new TeacherProfile { Id = 3, UserId = 5, FullName = "Le Van Duc", Bio = "Thạc sĩ Hóa học, chuyên ôn luyện Hóa & Sinh cho học sinh THPT. Phương pháp dạy trực quan, dễ hiểu.", Subjects = "Science", HourlyRate = 35m, RatingAvg = 4.2m, TotalRatingCount = 28, CreatedAt = Now, UpdatedAt = Now },
            new TeacherProfile { Id = 4, UserId = 6, FullName = "Pham Thi Huong", Bio = "IELTS 8.0, 10 năm dạy Tiếng Anh giao tiếp và luyện thi IELTS/TOEIC. Cam kết đầu ra rõ ràng.", Subjects = "English", HourlyRate = 45m, RatingAvg = 4.8m, TotalRatingCount = 105, CreatedAt = Now, UpdatedAt = Now },
            new TeacherProfile { Id = 5, UserId = 7, FullName = "Nguyen Quoc Bao", Bio = "Kỹ sư phần mềm tại FPT Software, 5 năm dạy lập trình Python, C#, và Web Development cho mọi trình độ.", Subjects = "Coding", HourlyRate = 55m, RatingAvg = 4.6m, TotalRatingCount = 67, CreatedAt = Now, UpdatedAt = Now },
            new TeacherProfile { Id = 6, UserId = 8, FullName = "Hoang Thi Lan", Bio = "Giáo viên Ngữ Văn & Lịch Sử THPT Quốc Gia. Chuyên luyện đề thi và viết văn nghị luận xã hội.", Subjects = "English", HourlyRate = 22m, RatingAvg = 4.0m, TotalRatingCount = 19, CreatedAt = Now, UpdatedAt = Now },
            new TeacherProfile { Id = 7, UserId = 9, FullName = "Vu Minh Khoa", Bio = "Tiến sĩ Toán ứng dụng, cựu giảng viên ĐH Bách Khoa. Dạy Toán cao cấp, Thống kê và Tin học.", Subjects = "Mathematics,Coding", HourlyRate = 40m, RatingAvg = 4.7m, TotalRatingCount = 83, CreatedAt = Now, UpdatedAt = Now },
            new TeacherProfile { Id = 8, UserId = 10, FullName = "Do Thi Thu", Bio = "Tốt nghiệp ĐH Ngoại Ngữ, dạy Tiếng Anh và Tiếng Pháp. 8 năm kinh nghiệm, lớp học tương tác cao.", Subjects = "English,Languages", HourlyRate = 38m, RatingAvg = 4.3m, TotalRatingCount = 51, CreatedAt = Now, UpdatedAt = Now },
            new TeacherProfile { Id = 9, UserId = 11, FullName = "Bui Van Long", Bio = "Cựu học sinh Chuyên Lý ĐH Khoa Học Tự Nhiên. Dạy Vật Lý và Hóa học theo hướng tư duy phân tích.", Subjects = "Science,Mathematics", HourlyRate = 30m, RatingAvg = 4.1m, TotalRatingCount = 33, CreatedAt = Now, UpdatedAt = Now },
            new TeacherProfile { Id = 10, UserId = 12, FullName = "Nguyen Thi Bich", Bio = "Giáo viên Toán chuyên, huy chương Bạc Olympic Toán quốc gia. Đam mê giúp học sinh yêu thích Toán học.", Subjects = "Mathematics", HourlyRate = 25m, RatingAvg = 4.9m, TotalRatingCount = 134, CreatedAt = Now, UpdatedAt = Now },
            new TeacherProfile { Id = 11, UserId = 13, FullName = "Trinh Van Nam", Bio = "Senior Developer 8 năm kinh nghiệm, chuyên dạy Lập trình và Toán rời rạc cho sinh viên CNTT.", Subjects = "Coding,Mathematics", HourlyRate = 65m, RatingAvg = 4.4m, TotalRatingCount = 58, CreatedAt = Now, UpdatedAt = Now }
        );

        m.Entity<StudentProfile>().HasData(
            new StudentProfile
            {
                Id = 1,
                UserId = 3,
                FullName = "Nguyen Minh Tuan",
                GradeLevel = "12",
                StudyStreakDays = 8,
                CreatedAt = Now,
                UpdatedAt = Now,
            },
            new StudentProfile { Id = 2, UserId = 14, FullName = "Le Thi Hoa", GradeLevel = "11", StudyStreakDays = 3, CreatedAt = Now, UpdatedAt = Now },
            new StudentProfile { Id = 3, UserId = 15, FullName = "Pham Van Kien", GradeLevel = "10", StudyStreakDays = 1, CreatedAt = Now, UpdatedAt = Now }
        );
    }

    // ─────────────────────────────────────────────────────────────────────────
    private static void SeedClasses(ModelBuilder m)
    {
        m.Entity<Class>().HasData(
            new Class
            {
                Id = 1,
                TeacherId = 2,
                Name = "Toán 12 — Luyện thi THPT",
                Description = "Ôn luyện toàn bộ chương trình Toán lớp 12.",
                Subject = "Toán",
                Status = ClassStatus.ACTIVE,
                MaxStudents = 20,
                StartDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                TotalSessions = 24,
                ThumbnailUrl = "https://placehold.co/400?text=Course",
                CreatedAt = Now,
                UpdatedAt = Now,
            }
        );
    }

    // ─────────────────────────────────────────────────────────────────────────
    private static void SeedClassMembers(ModelBuilder m)
    {
        m.Entity<ClassMember>().HasData(
            new ClassMember
            {
                Id = 1,
                ClassId = 1,
                StudentId = 3,
                Status = ClassMemberStatus.ACTIVE,
                JoinedAt = Now,
            }
        );
    }

    // ─────────────────────────────────────────────────────────────────────────
    private static void SeedSchedules(ModelBuilder m)
    {
        // 9/3/2026 7:30-9:30 VN (00:30-02:30 UTC)
        // 9/3/2026 12:00-13:00 VN (05:00-06:00 UTC)
        m.Entity<Schedule>().HasData(
            new Schedule
            {
                Id = 101,
                ClassId = 1,
                Title = "Buổi 1 sáng",
                StartTime = new DateTime(2026, 3, 9, 0, 30, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2026, 3, 9, 2, 30, 0, DateTimeKind.Utc),
                Type = ScheduleType.REGULAR,
                Status = ScheduleStatus.SCHEDULED,
                CreatedAt = Now,
                UpdatedAt = Now
            },
            new Schedule
            {
                Id = 102,
                ClassId = 1,
                Title = "Buổi 2 trưa",
                StartTime = new DateTime(2026, 3, 9, 5, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2026, 3, 9, 6, 0, 0, DateTimeKind.Utc),
                Type = ScheduleType.REGULAR,
                Status = ScheduleStatus.SCHEDULED,
                CreatedAt = Now,
                UpdatedAt = Now
            }
        );
    }
}
