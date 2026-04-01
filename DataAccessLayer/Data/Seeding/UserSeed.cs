using BusinessObject.Enum;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data.Seeding;

/// <summary>
/// Seed data cho student (user_id=3):
///   - Classes  : PRN222 (id=2), PRU213 (id=3) — teacher user_id=2
///   - Members  : student_id=3 ACTIVE trong cả 2 lớp
///   - Schedules: PRN222 Thứ 2+4, PRU213 Thứ 3+5 — tuần 09/03/2026
///   - Assignments, Materials, Notifications
/// </summary>
public static class ClassSeed
{
    // Seed timestamp cố định (giống DataSeeder.Now)
    private static readonly DateTime Now = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    // Tuần 09/03/2026 — lịch học cố định (UTC, Vietnam = UTC+7)
    // PRN222 : Thứ 2 & Thứ 4  07:30–09:30 VN = 00:30–02:30 UTC
    // PRU213 : Thứ 3 & Thứ 5  13:00–15:00 VN = 06:00–08:00 UTC
    private static readonly DateTime Mon0930 = new(2026, 3, 9, 0, 30, 0, DateTimeKind.Utc); // PRN222 s1 start
    private static readonly DateTime Mon0930E = new(2026, 3, 9, 2, 30, 0, DateTimeKind.Utc); // PRN222 s1 end
    private static readonly DateTime Tue1300 = new(2026, 3, 10, 6, 0, 0, DateTimeKind.Utc); // PRU213 s1 start
    private static readonly DateTime Tue1300E = new(2026, 3, 10, 8, 0, 0, DateTimeKind.Utc); // PRU213 s1 end
    private static readonly DateTime Wed0930 = new(2026, 3, 11, 0, 30, 0, DateTimeKind.Utc); // PRN222 s2 start
    private static readonly DateTime Wed0930E = new(2026, 3, 11, 2, 30, 0, DateTimeKind.Utc); // PRN222 s2 end
    private static readonly DateTime Thu1300 = new(2026, 3, 12, 6, 0, 0, DateTimeKind.Utc); // PRU213 s2 start
    private static readonly DateTime Thu1300E = new(2026, 3, 12, 8, 0, 0, DateTimeKind.Utc); // PRU213 s2 end

    public static void Seed(ModelBuilder m)
    {
        SeedClasses(m);
        SeedClassMembers(m);
        SeedSchedules(m);
        SeedAssignments(m);
        SeedAssignmentQuestions(m);
        SeedSubmissions(m);
        SeedFeedbacks(m);
        SeedMaterials(m);
        SeedNotifications(m);
    }

    // ── Classes ───────────────────────────────────────────────────────────────
    private static void SeedClasses(ModelBuilder m)
    {
        m.Entity<Class>()
            .HasData(
                new Class
                {
                    Id = 2,
                    TeacherId = 2, // teacher@learnmate.vn
                    Name = "PRN222",
                    Description = "Cross-Platform Mobile App Development With .NET MAUI",
                    Subject = "PRN222",
                    Status = ClassStatus.ACTIVE,
                    MaxStudents = 30,
                    CreatedAt = Now,
                    UpdatedAt = Now,
                },
                new Class
                {
                    Id = 3,
                    TeacherId = 2,
                    Name = "PRU213",
                    Description = "Unity Game Development — Scripting & Physics",
                    Subject = "PRU213",
                    Status = ClassStatus.ACTIVE,
                    MaxStudents = 30,
                    CreatedAt = Now,
                    UpdatedAt = Now,
                }
            );
    }

    // ── Class members ──────────────────────────────────────────────────────────
    private static void SeedClassMembers(ModelBuilder m)
    {
        m.Entity<ClassMember>()
            .HasData(
                new ClassMember
                {
                    Id = 2,
                    ClassId = 2, // PRN222
                    StudentId = 3, // student@learnmate.vn
                    Status = ClassMemberStatus.ACTIVE,
                    JoinedAt = Now,
                },
                new ClassMember
                {
                    Id = 3,
                    ClassId = 3, // PRU213
                    StudentId = 3,
                    Status = ClassMemberStatus.ACTIVE,
                    JoinedAt = Now,
                }
            );
    }

    // ── Schedules — tuần 09/03/2026 ──────────────────────────────────────────
    private static void SeedSchedules(ModelBuilder m)
    {
        m.Entity<Schedule>()
            .HasData(
                // PRN222 — Thứ 2 (09/03) 07:30–09:30 VN
                new Schedule
                {
                    Id = 1,
                    ClassId = 2,
                    Title = "PRN222 — Buổi 1: Giới thiệu .NET MAUI & XAML",
                    StartTime = Mon0930,
                    EndTime = Mon0930E,
                    Type = ScheduleType.REGULAR,
                    Status = ScheduleStatus.SCHEDULED,
                    IsTrial = false,
                    CreatedAt = Now,
                    UpdatedAt = Now,
                },
                // PRN222 — Thứ 4 (11/03) 07:30–09:30 VN
                new Schedule
                {
                    Id = 2,
                    ClassId = 2,
                    Title = "PRN222 — Buổi 2: Data Binding & MVVM",
                    StartTime = Wed0930,
                    EndTime = Wed0930E,
                    Type = ScheduleType.REGULAR,
                    Status = ScheduleStatus.SCHEDULED,
                    IsTrial = false,
                    CreatedAt = Now,
                    UpdatedAt = Now,
                },
                // PRU213 — Thứ 3 (10/03) 13:00–15:00 VN
                new Schedule
                {
                    Id = 3,
                    ClassId = 3,
                    Title = "PRU213 — Buổi 1: Unity Editor & Scene Setup",
                    StartTime = Tue1300,
                    EndTime = Tue1300E,
                    Type = ScheduleType.REGULAR,
                    Status = ScheduleStatus.SCHEDULED,
                    IsTrial = false,
                    CreatedAt = Now,
                    UpdatedAt = Now,
                },
                // PRU213 — Thứ 5 (12/03) 13:00–15:00 VN
                new Schedule
                {
                    Id = 4,
                    ClassId = 3,
                    Title = "PRU213 — Buổi 2: Rigidbody Physics & Colliders",
                    StartTime = Thu1300,
                    EndTime = Thu1300E,
                    Type = ScheduleType.REGULAR,
                    Status = ScheduleStatus.SCHEDULED,
                    IsTrial = false,
                    CreatedAt = Now,
                    UpdatedAt = Now,
                }
            );
    }

    // ── Assignments ───────────────────────────────────────────────────────────
    private static void SeedAssignments(ModelBuilder m)
    {
        m.Entity<Assignment>()
            .HasData(
                new Assignment
                {
                    Id = 1,
                    ClassId = 2, // PRN222
                    TeacherId = 2,
                    Title = "Lab 1: MAUI Navigation",
                    Description =
                        "Xây dựng ứng dụng MAUI có ít nhất 3 màn hình với Shell Navigation và truyền dữ liệu giữa các trang.",
                    Status = AssignmentStatus.PUBLISHED,
                    DueDate = new DateTime(2026, 3, 16, 16, 59, 0, DateTimeKind.Utc), // 23:59 VN
                    CreatedAt = Now,
                    UpdatedAt = Now,
                },
                new Assignment
                {
                    Id = 2,
                    ClassId = 3, // PRU213
                    TeacherId = 2,
                    Title = "Lab 1: Unity Scene",
                    Description =
                        "Tạo một scene Unity có ánh sáng directional, mặt phẳng (Plane), và ít nhất 1 GameObject có Rigidbody và Collider.",
                    Status = AssignmentStatus.PUBLISHED,
                    DueDate = new DateTime(2026, 3, 15, 16, 59, 0, DateTimeKind.Utc), // 23:59 VN
                    CreatedAt = Now,
                    UpdatedAt = Now,
                }
            );
    }

    // ── Assignment Questions ──────────────────────────────────────────────────
    private static void SeedAssignmentQuestions(ModelBuilder m)
    {
        m.Entity<AssignmentQuestion>()
            .HasData(
                new AssignmentQuestion
                {
                    Id = 1,
                    AssignmentId = 1, // Lab 1: MAUI Navigation
                    Content = "Triển khai Shell Navigation giữa ít nhất 3 màn hình.",
                    Type = QuestionType.ESSAY,
                    Order = 1,
                    Points = 5m,
                    CreatedAt = Now,
                    UpdatedAt = Now,
                },
                new AssignmentQuestion
                {
                    Id = 2,
                    AssignmentId = 1,
                    Content =
                        "Truyền dữ liệu giữa các trang bằng QueryProperty hoặc constructor injection.",
                    Type = QuestionType.ESSAY,
                    Order = 2,
                    Points = 5m,
                    CreatedAt = Now,
                    UpdatedAt = Now,
                },
                new AssignmentQuestion
                {
                    Id = 3,
                    AssignmentId = 2, // Lab 1: Unity Scene
                    Content = "Tạo scene Unity với đầy đủ ánh sáng, Plane, Rigidbody và Collider.",
                    Type = QuestionType.ESSAY,
                    Order = 1,
                    Points = 10m,
                    CreatedAt = Now,
                    UpdatedAt = Now,
                }
            );
    }

    // ── Submissions ───────────────────────────────────────────────────────────
    private static void SeedSubmissions(ModelBuilder m)
    {
        m.Entity<Submission>()
            .HasData(
                new Submission
                {
                    Id = 1,
                    AssignmentId = 1, // Lab 1: MAUI Navigation — đã chấm
                    StudentId = 3,
                    Status = SubmissionStatus.GRADED,
                    Score = 8.5m,
                    SubmittedAt = new DateTime(2026, 3, 14, 10, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2026, 3, 14, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 3, 15, 8, 0, 0, DateTimeKind.Utc),
                },
                new Submission
                {
                    Id = 2,
                    AssignmentId = 2, // Lab 1: Unity Scene — đã nộp, chưa chấm
                    StudentId = 3,
                    Status = SubmissionStatus.SUBMITTED,
                    SubmittedAt = new DateTime(2026, 3, 14, 15, 30, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2026, 3, 14, 15, 30, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 3, 14, 15, 30, 0, DateTimeKind.Utc),
                }
            );
    }

    // ── Feedbacks ─────────────────────────────────────────────────────────────
    private static void SeedFeedbacks(ModelBuilder m)
    {
        m.Entity<Feedback>()
            .HasData(
                new Feedback
                {
                    Id = 1,
                    SubmissionId = 1,
                    Score = 8.5m,
                    Comment =
                        "Bài làm tốt! Navigation giữa các màn hình hoạt động đúng yêu cầu. Tuy nhiên, cần cải thiện phần truyền dữ liệu — nên dùng QueryProperty thay vì singleton. Tiếp tục cố gắng nhé!",
                    CreatedAt = new DateTime(2026, 3, 15, 8, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 3, 15, 8, 0, 0, DateTimeKind.Utc),
                }
            );
    }

    // ── Materials ─────────────────────────────────────────────────────────────
    private static void SeedMaterials(ModelBuilder m)
    {
        m.Entity<Material>()
            .HasData(
                new Material
                {
                    Id = 1,
                    ClassId = 2, // PRN222
                    UploadedBy = 2, // teacher
                    Title = "Slide Buổi 1 — .NET MAUI Introduction",
                    Description = "Tổng quan về .NET MAUI, XAML cơ bản và cấu trúc project.",
                    FileUrl = "https://storage.learnmate.vn/materials/prn222-slide-01.pdf",
                    FileType = "PDF",
                    Status = MaterialStatus.ACTIVE,
                    CreatedAt = Now,
                    UpdatedAt = Now,
                },
                new Material
                {
                    Id = 2,
                    ClassId = 3, // PRU213
                    UploadedBy = 2,
                    Title = "Slide Buổi 1 — Unity Editor Overview",
                    Description =
                        "Hướng dẫn cài đặt Unity Hub, tạo project và làm quen với Editor.",
                    FileUrl = "https://storage.learnmate.vn/materials/pru213-slide-01.pdf",
                    FileType = "PDF",
                    Status = MaterialStatus.ACTIVE,
                    CreatedAt = Now,
                    UpdatedAt = Now,
                }
            );
    }

    // ── Notifications ─────────────────────────────────────────────────────────
    private static void SeedNotifications(ModelBuilder m)
    {
        m.Entity<Notification>()
            .HasData(
                new Notification
                {
                    Id = 1,
                    UserId = 3,
                    Title = "Chào mừng đến PRN222",
                    Content =
                        "Bạn đã tham gia lớp Cross-Platform Mobile App Development thành công!",
                    IsRead = true,
                    CreatedAt = Now,
                    UpdatedAt = Now,
                },
                new Notification
                {
                    Id = 2,
                    UserId = 3,
                    Title = "Chào mừng đến PRU213",
                    Content = "Bạn đã tham gia lớp Unity Game Development thành công!",
                    IsRead = true,
                    CreatedAt = Now,
                    UpdatedAt = Now,
                },
                new Notification
                {
                    Id = 3,
                    UserId = 3,
                    Title = "Bài tập mới — PRN222",
                    Content = "Lab 1: MAUI Navigation đã được đăng. Hạn nộp: 16/03/2026.",
                    IsRead = false,
                    CreatedAt = new DateTime(2026, 3, 7, 1, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 3, 7, 1, 0, 0, DateTimeKind.Utc),
                },
                new Notification
                {
                    Id = 4,
                    UserId = 3,
                    Title = "Bài tập mới — PRU213",
                    Content = "Lab 1: Unity Scene đã được đăng. Hạn nộp: 15/03/2026.",
                    IsRead = false,
                    CreatedAt = new DateTime(2026, 3, 7, 2, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 3, 7, 2, 0, 0, DateTimeKind.Utc),
                },
                new Notification
                {
                    Id = 5,
                    UserId = 3,
                    Title = "Lịch học tuần này",
                    Content = "PRN222: Thứ 2 & Thứ 4 lúc 07:30. PRU213: Thứ 3 & Thứ 5 lúc 13:00.",
                    IsRead = false,
                    CreatedAt = new DateTime(2026, 3, 8, 3, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 3, 8, 3, 0, 0, DateTimeKind.Utc),
                }
            );
    }
}
