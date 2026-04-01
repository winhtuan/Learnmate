using BusinessObject.Enum;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data.Seeding;

/// <summary>
/// Seed lớp học cho 10 giáo viên bổ sung (user_id=4..13).
/// Mỗi giáo viên có 2 lớp, mỗi lớp có 2 buổi/tuần trong tuần 09/03/2026.
///
/// IDs được dùng:
///   Class        : 4-23
///   Schedule     : 5-44
///   ClassMember  : 4-13
/// </summary>
public static class TutorClassSeed
{
    private static readonly DateTime Now = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    // ── Tuần 09/03/2026 — các khung giờ (UTC, Vietnam = UTC+7) ───────────────
    // Morning:   07:30-09:30 VN = 00:30-02:30 UTC
    // Afternoon: 14:00-16:00 VN = 07:00-09:00 UTC
    // Evening:   18:00-20:00 VN = 11:00-13:00 UTC
    // Night:     19:00-21:00 VN = 12:00-14:00 UTC
    private static DateTime D(int day, int utcHour) => new(2026, 3, day, utcHour, 0, 0, DateTimeKind.Utc);

    public static void Seed(ModelBuilder m)
    {
        SeedClasses(m);
        SeedSchedules(m);
        SeedClassMembers(m);
    }

    // ── Classes — id=4..23 ────────────────────────────────────────────────────
    private static void SeedClasses(ModelBuilder m)
    {
        m.Entity<Class>().HasData(

            // ── Tran Thi Mai (UserId=4) — Mathematics, Science ────────────────
            new Class { Id = 4, TeacherId = 4, Name = "Toán 12 — Luyện thi THPT Quốc Gia", Subject = "Mathematics", Description = "Ôn luyện toàn bộ chương trình Toán 12: Giới hạn, Đạo hàm, Tích phân, Hình học không gian. Tập trung vào dạng đề thi THPT QG.", Status = ClassStatus.ACTIVE, MaxStudents = 20, StartDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 24, ThumbnailUrl = "https://placehold.co/400/fce7f3/be185d?text=T12", CreatedAt = Now, UpdatedAt = Now },
            new Class { Id = 5, TeacherId = 4, Name = "Vật Lý 11 — Điện và Từ Trường", Subject = "Science", Description = "Chuyên sâu Điện học và Từ trường lớp 11: điện tích, tụ điện, định luật Ohm, từ trường, cảm ứng điện từ.", Status = ClassStatus.ACTIVE, MaxStudents = 15, StartDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 24, ThumbnailUrl = "https://placehold.co/400/fce7f3/be185d?text=VL11", CreatedAt = Now, UpdatedAt = Now },

            // ── Le Van Duc (UserId=5) — Science ───────────────────────────────
            new Class { Id = 6, TeacherId = 5, Name = "Hóa Học 12 — Hữu Cơ & Vô Cơ", Subject = "Science", Description = "Hệ thống hóa toàn bộ Hóa 12: Polymer, Este-Lipid, Amin-Aminoaxit, Kim loại kiềm, Kim loại chuyển tiếp. Luyện đề THPT.", Status = ClassStatus.ACTIVE, MaxStudents = 18, StartDate = new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 8, 15, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 32, ThumbnailUrl = "https://placehold.co/400/dcfce7/16a34a?text=HH12", CreatedAt = Now, UpdatedAt = Now },
            new Class { Id = 7, TeacherId = 5, Name = "Sinh Học 11 — Sinh Lý Thực Vật & Động Vật", Subject = "Science", Description = "Sinh lý thực vật: trao đổi chất, quang hợp, hô hấp. Sinh lý động vật: tuần hoàn, hô hấp, bài tiết, thần kinh.", Status = ClassStatus.ACTIVE, MaxStudents = 15, StartDate = new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 8, 15, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 32, ThumbnailUrl = "https://placehold.co/400/dcfce7/16a34a?text=SH11", CreatedAt = Now, UpdatedAt = Now },

            // ── Pham Thi Huong (UserId=6) — English ───────────────────────────
            new Class { Id = 8, TeacherId = 6, Name = "IELTS Preparation — Band 6.5+", Subject = "English", Description = "Luyện thi IELTS toàn diện 4 kỹ năng: Listening, Reading, Writing Task 1&2, Speaking. Cam kết đầu ra Band 6.5 sau 3 tháng.", Status = ClassStatus.ACTIVE, MaxStudents = 12, StartDate = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 48, ThumbnailUrl = "https://placehold.co/400/fef9c3/ca8a04?text=IELTS", CreatedAt = Now, UpdatedAt = Now },
            new Class { Id = 9, TeacherId = 6, Name = "English Communication — Intermediate", Subject = "English", Description = "Phát triển kỹ năng giao tiếp tiếng Anh tự nhiên: phát âm, ngữ điệu, từ vựng thực tế. Phù hợp trình độ A2-B1.", Status = ClassStatus.ACTIVE, MaxStudents = 15, StartDate = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 48, ThumbnailUrl = "https://placehold.co/400/fef9c3/ca8a04?text=ENG", CreatedAt = Now, UpdatedAt = Now },

            // ── Nguyen Quoc Bao (UserId=7) — Coding ───────────────────────────
            new Class { Id = 10, TeacherId = 7, Name = "Python Cơ Bản đến Nâng Cao", Subject = "Coding", Description = "Từ cú pháp Python đến OOP, xử lý file, API, và Data Science cơ bản với Pandas/NumPy. Thực hành dự án thực tế.", Status = ClassStatus.ACTIVE, MaxStudents = 20, StartDate = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 6, 20, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 12, ThumbnailUrl = "https://placehold.co/400/ede9fe/7c3aed?text=PY", CreatedAt = Now, UpdatedAt = Now },
            new Class { Id = 11, TeacherId = 7, Name = "Web Development với ASP.NET Core", Subject = "Coding", Description = "Xây dựng Web API RESTful với ASP.NET Core 8, Entity Framework Core, JWT Auth, và deploy lên Azure. Thực hành project cuối khóa.", Status = ClassStatus.ACTIVE, MaxStudents = 15, StartDate = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 6, 20, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 12, ThumbnailUrl = "https://placehold.co/400/ede9fe/7c3aed?text=NET", CreatedAt = Now, UpdatedAt = Now },

            // ── Hoang Thi Lan (UserId=8) — Văn & Sử ──────────────────────────
            new Class { Id = 12, TeacherId = 8, Name = "Ngữ Văn 12 — Nghị Luận Xã Hội & Văn Học", Subject = "English", Description = "Luyện viết văn nghị luận xã hội và phân tích tác phẩm văn học lớp 12. Bí quyết đạt điểm cao môn Văn THPT QG.", Status = ClassStatus.ACTIVE, MaxStudents = 20, StartDate = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 16, ThumbnailUrl = "https://placehold.co/400/ffedd5/ea580c?text=VAN", CreatedAt = Now, UpdatedAt = Now },
            new Class { Id = 13, TeacherId = 8, Name = "Lịch Sử 12 — Ôn thi THPT Quốc Gia", Subject = "English", Description = "Hệ thống toàn bộ Lịch Sử 12: Lịch sử thế giới, Lịch sử Việt Nam 1919-2000. Luyện câu hỏi tư duy và ghi nhớ nhanh.", Status = ClassStatus.ACTIVE, MaxStudents = 18, StartDate = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 16, ThumbnailUrl = "https://placehold.co/400/ffedd5/ea580c?text=LS", CreatedAt = Now, UpdatedAt = Now },

            // ── Vu Minh Khoa (UserId=9) — Mathematics, Coding ─────────────────
            new Class { Id = 14, TeacherId = 9, Name = "Toán Cao Cấp A1 & A2 cho Sinh viên", Subject = "Mathematics", Description = "Giải tích hàm một biến và nhiều biến, Đại số tuyến tính, Phương trình vi phân. Phù hợp sinh viên ĐH Kỹ thuật.", Status = ClassStatus.ACTIVE, MaxStudents = 25, StartDate = new DateTime(2026, 7, 10, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 9, 30, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 24, ThumbnailUrl = "https://placehold.co/400/cffafe/0891b2?text=TCC", CreatedAt = Now, UpdatedAt = Now },
            new Class { Id = 15, TeacherId = 9, Name = "Thống Kê Xác Suất & Ứng Dụng", Subject = "Mathematics", Description = "Xác suất, Phân phối xác suất, Kiểm định giả thuyết, Hồi quy tuyến tính. Ứng dụng trong Data Science và Kinh tế.", Status = ClassStatus.ACTIVE, MaxStudents = 20, StartDate = new DateTime(2026, 7, 10, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 9, 30, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 24, ThumbnailUrl = "https://placehold.co/400/cffafe/0891b2?text=TK", CreatedAt = Now, UpdatedAt = Now },

            // ── Do Thi Thu (UserId=10) — English, Languages ────────────────────
            new Class { Id = 16, TeacherId = 10, Name = "Tiếng Anh Giao Tiếp — Beginner A1-A2", Subject = "English", Description = "Khóa học tiếng Anh giao tiếp dành cho người mới bắt đầu. Xây dựng vốn từ vựng, phát âm chuẩn và phản xạ hội thoại cơ bản.", Status = ClassStatus.ACTIVE, MaxStudents = 12, StartDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 11, 30, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 32, ThumbnailUrl = "https://placehold.co/400/fce7f3/db2777?text=ENG", CreatedAt = Now, UpdatedAt = Now },
            new Class { Id = 17, TeacherId = 10, Name = "Tiếng Pháp Cơ Bản A1-A2 (DELF)", Subject = "English", Description = "Tiếng Pháp từ đầu: ngữ âm, ngữ pháp cơ bản, hội thoại thực tế. Chuẩn bị cho kỳ thi DELF A1 và A2.", Status = ClassStatus.ACTIVE, MaxStudents = 10, StartDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 11, 30, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 32, ThumbnailUrl = "https://placehold.co/400/fce7f3/db2777?text=FR", CreatedAt = Now, UpdatedAt = Now },

            // ── Bui Van Long (UserId=11) — Science, Mathematics ────────────────
            new Class { Id = 18, TeacherId = 11, Name = "Vật Lý 12 — Luyện thi THPT Quốc Gia", Subject = "Science", Description = "Dao động, Sóng, Điện xoay chiều, Lượng tử ánh sáng, Hạt nhân nguyên tử. Luyện đề THPT QG theo cấu trúc mới.", Status = ClassStatus.ACTIVE, MaxStudents = 20, StartDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 5, 15, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 40, ThumbnailUrl = "https://placehold.co/400/e0f2fe/0284c7?text=VL12", CreatedAt = Now, UpdatedAt = Now },
            new Class { Id = 19, TeacherId = 11, Name = "Hóa Học 11 — Hóa Hữu Cơ Cơ Bản", Subject = "Science", Description = "Đại cương hóa hữu cơ, Hydrocarbon, Dẫn xuất Halogen, Ancol-Phenol-Ete, Andehit-Axit Cacboxylic. Bài tập từ cơ bản đến nâng cao.", Status = ClassStatus.ACTIVE, MaxStudents = 18, StartDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 5, 15, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 40, ThumbnailUrl = "https://placehold.co/400/e0f2fe/0284c7?text=HH11", CreatedAt = Now, UpdatedAt = Now },

            // ── Nguyen Thi Bich (UserId=12) — Mathematics ─────────────────────
            new Class { Id = 20, TeacherId = 12, Name = "Toán Chuyên — Luyện thi Olympic & Đại học", Subject = "Mathematics", Description = "Dành cho học sinh chuyên Toán và HSG: Số học, Đại số, Hình học Euclidean, Tổ hợp. Luyện đề Olympic cấp tỉnh và quốc gia.", Status = ClassStatus.ACTIVE, MaxStudents = 10, StartDate = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 20, ThumbnailUrl = "https://placehold.co/400/f1f5f9/475569?text=OLP", CreatedAt = Now, UpdatedAt = Now },
            new Class { Id = 21, TeacherId = 12, Name = "Toán 10 — Đại Số, Hình Học & Lượng Giác", Subject = "Mathematics", Description = "Xây dựng nền tảng Toán lớp 10 vững chắc: Mệnh đề-Tập hợp, Hàm số, Phương trình-Bất phương trình, Hình học phẳng, Lượng giác.", Status = ClassStatus.ACTIVE, MaxStudents = 20, StartDate = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 20, ThumbnailUrl = "https://placehold.co/400/f1f5f9/475569?text=T10", CreatedAt = Now, UpdatedAt = Now },

            // ── Trinh Van Nam (UserId=13) — Coding, Mathematics ───────────────
            new Class { Id = 22, TeacherId = 13, Name = "Lập Trình C# — .NET Full Stack Developer", Subject = "Coding", Description = "C# từ cơ bản đến nâng cao, ASP.NET Core MVC, EF Core, SQL Server, React + Blazor. Xây dựng portfolio project thực tế.", Status = ClassStatus.ACTIVE, MaxStudents = 15, StartDate = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2027, 2, 28, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 48, ThumbnailUrl = "https://placehold.co/400/fef2f2/dc2626?text=CS", CreatedAt = Now, UpdatedAt = Now },
            new Class { Id = 23, TeacherId = 13, Name = "Toán Rời Rạc cho Sinh viên CNTT", Subject = "Mathematics", Description = "Logic mệnh đề, Lý thuyết tập hợp, Quan hệ, Hàm, Lý thuyết đồ thị, Đếm tổ hợp, Xác suất rời rạc. Ứng dụng trong Khoa học máy tính.", Status = ClassStatus.ACTIVE, MaxStudents = 20, StartDate = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2027, 2, 28, 0, 0, 0, DateTimeKind.Utc), TotalSessions = 48, ThumbnailUrl = "https://placehold.co/400/fef2f2/dc2626?text=TRR", CreatedAt = Now, UpdatedAt = Now }
        );
    }

    // ── Schedules — id=5..44 ─────────────────────────────────────────────────
    // Hai buổi/tuần, tuần 09/03/2026. Định dạng: D(ngày trong tháng 3, giờ UTC)
    private static void SeedSchedules(ModelBuilder m)
    {
        m.Entity<Schedule>().HasData(

            // Class 4 (Toán 12 — Tran Thi Mai): Thứ 3 & Thứ 5  18:00-20:00 VN = 11:00-13:00 UTC
            new Schedule { Id = 5, ClassId = 4, Title = "Toán 12 — Buổi 1: Giới hạn & Liên tục", StartTime = D(10, 11), EndTime = D(10, 13), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 6, ClassId = 4, Title = "Toán 12 — Buổi 2: Đạo hàm & Ứng dụng", StartTime = D(12, 11), EndTime = D(12, 13), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 5 (Vật Lý 11 — Tran Thi Mai): Thứ 2 & Thứ 4  18:00-20:00 VN = 11:00-13:00 UTC
            new Schedule { Id = 7, ClassId = 5, Title = "Vật Lý 11 — Buổi 1: Tĩnh điện học", StartTime = D(9, 11), EndTime = D(9, 13), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 8, ClassId = 5, Title = "Vật Lý 11 — Buổi 2: Điện trường & Tụ điện", StartTime = D(11, 11), EndTime = D(11, 13), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 6 (Hóa Học 12 — Le Van Duc): Thứ 3 & Thứ 5  07:30-09:30 VN = 00:30-02:30 UTC
            new Schedule { Id = 9, ClassId = 6, Title = "Hóa 12 — Buổi 1: Este và Lipit", StartTime = D(10, 0), EndTime = D(10, 2), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 10, ClassId = 6, Title = "Hóa 12 — Buổi 2: Cacbohidrat", StartTime = D(12, 0), EndTime = D(12, 2), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 7 (Sinh Học 11 — Le Van Duc): Thứ 2 & Thứ 4  07:30-09:30 VN = 00:30-02:30 UTC
            new Schedule { Id = 11, ClassId = 7, Title = "Sinh 11 — Buổi 1: Trao đổi nước ở thực vật", StartTime = D(9, 0), EndTime = D(9, 2), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 12, ClassId = 7, Title = "Sinh 11 — Buổi 2: Quang hợp", StartTime = D(11, 0), EndTime = D(11, 2), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 8 (IELTS — Pham Thi Huong): Thứ 2 & Thứ 4  19:00-21:00 VN = 12:00-14:00 UTC
            new Schedule { Id = 13, ClassId = 8, Title = "IELTS — Buổi 1: Reading Strategies", StartTime = D(9, 12), EndTime = D(9, 14), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 14, ClassId = 8, Title = "IELTS — Buổi 2: Writing Task 2 Structure", StartTime = D(11, 12), EndTime = D(11, 14), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 9 (English Communication — Pham Thi Huong): Thứ 3 & Thứ 5  19:00-21:00 VN = 12:00-14:00 UTC
            new Schedule { Id = 15, ClassId = 9, Title = "English Comm — Buổi 1: Greetings & Small Talk", StartTime = D(10, 12), EndTime = D(10, 14), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 16, ClassId = 9, Title = "English Comm — Buổi 2: Describing People", StartTime = D(12, 12), EndTime = D(12, 14), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 10 (Python — Nguyen Quoc Bao): Thứ 2 & Thứ 5  18:00-20:00 VN = 11:00-13:00 UTC
            new Schedule { Id = 17, ClassId = 10, Title = "Python — Buổi 1: Biến, Kiểu dữ liệu, Vòng lặp", StartTime = D(9, 11), EndTime = D(9, 13), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 18, ClassId = 10, Title = "Python — Buổi 2: Hàm & Module", StartTime = D(12, 11), EndTime = D(12, 13), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 11 (ASP.NET Core — Nguyen Quoc Bao): Thứ 3 & Thứ 6  18:00-20:00 VN = 11:00-13:00 UTC
            new Schedule { Id = 19, ClassId = 11, Title = "ASP.NET — Buổi 1: Cấu trúc project & DI", StartTime = D(10, 11), EndTime = D(10, 13), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 20, ClassId = 11, Title = "ASP.NET — Buổi 2: RESTful API & Routing", StartTime = D(13, 11), EndTime = D(13, 13), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 12 (Ngữ Văn — Hoang Thi Lan): Thứ 7 & CN  08:00-10:00 VN = 01:00-03:00 UTC
            new Schedule { Id = 21, ClassId = 12, Title = "Ngữ Văn — Buổi 1: Nghị luận xã hội", StartTime = D(14, 1), EndTime = D(14, 3), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 22, ClassId = 12, Title = "Ngữ Văn — Buổi 2: Phân tích thơ Tây Tiến", StartTime = D(15, 1), EndTime = D(15, 3), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 13 (Lịch Sử — Hoang Thi Lan): Thứ 7 & CN  14:00-16:00 VN = 07:00-09:00 UTC
            new Schedule { Id = 23, ClassId = 13, Title = "Lịch Sử — Buổi 1: Liên Xô & Đông Âu 1945-1991", StartTime = D(14, 7), EndTime = D(14, 9), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 24, ClassId = 13, Title = "Lịch Sử — Buổi 2: Cách mạng Việt Nam 1930-1945", StartTime = D(15, 7), EndTime = D(15, 9), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 14 (Toán Cao Cấp — Vu Minh Khoa): Thứ 2 & Thứ 4  19:00-21:00 VN = 12:00-14:00 UTC
            new Schedule { Id = 25, ClassId = 14, Title = "Toán CC — Buổi 1: Giới hạn & Đạo hàm hàm nhiều biến", StartTime = D(9, 12), EndTime = D(9, 14), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 26, ClassId = 14, Title = "Toán CC — Buổi 2: Tích phân bội", StartTime = D(11, 12), EndTime = D(11, 14), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 15 (Thống Kê — Vu Minh Khoa): Thứ 3 & Thứ 5  19:00-21:00 VN = 12:00-14:00 UTC
            new Schedule { Id = 27, ClassId = 15, Title = "Thống Kê — Buổi 1: Biến ngẫu nhiên & Phân phối", StartTime = D(10, 12), EndTime = D(10, 14), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 28, ClassId = 15, Title = "Thống Kê — Buổi 2: Ước lượng tham số", StartTime = D(12, 12), EndTime = D(12, 14), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 16 (Tiếng Anh Beginner — Do Thi Thu): Thứ 2 & Thứ 4  09:00-11:00 VN = 02:00-04:00 UTC
            new Schedule { Id = 29, ClassId = 16, Title = "Tiếng Anh — Buổi 1: Alphabet & Phonics", StartTime = D(9, 2), EndTime = D(9, 4), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 30, ClassId = 16, Title = "Tiếng Anh — Buổi 2: To be & Simple Present", StartTime = D(11, 2), EndTime = D(11, 4), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 17 (Tiếng Pháp — Do Thi Thu): Thứ 3 & Thứ 5  09:00-11:00 VN = 02:00-04:00 UTC
            new Schedule { Id = 31, ClassId = 17, Title = "Tiếng Pháp — Buổi 1: Bảng chữ cái & Ngữ âm", StartTime = D(10, 2), EndTime = D(10, 4), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 32, ClassId = 17, Title = "Tiếng Pháp — Buổi 2: Être & Avoir, Giới từ", StartTime = D(12, 2), EndTime = D(12, 4), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 18 (Vật Lý 12 — Bui Van Long): Thứ 2 & Thứ 4  17:00-19:00 VN = 10:00-12:00 UTC
            new Schedule { Id = 33, ClassId = 18, Title = "Vật Lý 12 — Buổi 1: Dao động điều hoà", StartTime = D(9, 10), EndTime = D(9, 12), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 34, ClassId = 18, Title = "Vật Lý 12 — Buổi 2: Sóng cơ học", StartTime = D(11, 10), EndTime = D(11, 12), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 19 (Hóa 11 — Bui Van Long): Thứ 3 & Thứ 5  17:00-19:00 VN = 10:00-12:00 UTC
            new Schedule { Id = 35, ClassId = 19, Title = "Hóa 11 — Buổi 1: Đại cương Hóa hữu cơ", StartTime = D(10, 10), EndTime = D(10, 12), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 36, ClassId = 19, Title = "Hóa 11 — Buổi 2: Ankan & Anken", StartTime = D(12, 10), EndTime = D(12, 12), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 20 (Toán Olympic — Nguyen Thi Bich): Thứ 7 & CN  09:00-11:00 VN = 02:00-04:00 UTC
            new Schedule { Id = 37, ClassId = 20, Title = "Toán Olympic — Buổi 1: Bất đẳng thức Cauchy-Schwarz", StartTime = D(14, 2), EndTime = D(14, 4), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 38, ClassId = 20, Title = "Toán Olympic — Buổi 2: Lý thuyết số cơ bản", StartTime = D(15, 2), EndTime = D(15, 4), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 21 (Toán 10 — Nguyen Thi Bich): Thứ 2 & Thứ 4  17:00-19:00 VN = 10:00-12:00 UTC
            new Schedule { Id = 39, ClassId = 21, Title = "Toán 10 — Buổi 1: Mệnh đề & Tập hợp", StartTime = D(9, 10), EndTime = D(9, 12), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 40, ClassId = 21, Title = "Toán 10 — Buổi 2: Hàm số & Đồ thị", StartTime = D(11, 10), EndTime = D(11, 12), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 22 (C# .NET — Trinh Van Nam): Thứ 2 & Thứ 5  19:00-21:00 VN = 12:00-14:00 UTC
            new Schedule { Id = 41, ClassId = 22, Title = "C# .NET — Buổi 1: OOP & SOLID Principles", StartTime = D(9, 12), EndTime = D(9, 14), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 42, ClassId = 22, Title = "C# .NET — Buổi 2: Async/Await & LINQ", StartTime = D(12, 12), EndTime = D(12, 14), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },

            // Class 23 (Toán Rời Rạc — Trinh Van Nam): Thứ 3 & Thứ 6  19:00-21:00 VN = 12:00-14:00 UTC
            new Schedule { Id = 43, ClassId = 23, Title = "Toán Rời Rạc — Buổi 1: Logic & Tập hợp", StartTime = D(10, 12), EndTime = D(10, 14), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now },
            new Schedule { Id = 44, ClassId = 23, Title = "Toán Rời Rạc — Buổi 2: Lý thuyết đồ thị", StartTime = D(13, 12), EndTime = D(13, 14), Type = ScheduleType.REGULAR, Status = ScheduleStatus.SCHEDULED, CreatedAt = Now, UpdatedAt = Now }
        );
    }

    // ── ClassMembers — id=4..13 ───────────────────────────────────────────────
    // student_id=14 (Le Thi Hoa)   : IELTS, Python, C# .NET
    // student_id=15 (Pham Van Kien) : IELTS, Toán Olympic, Toán Rời Rạc
    // student_id=3  (Nguyen Minh Tuan): Toán 12, Vật Lý 12
    private static void SeedClassMembers(ModelBuilder m)
    {
        m.Entity<ClassMember>().HasData(
            new ClassMember { Id = 4, ClassId = 8, StudentId = 14, Status = ClassMemberStatus.ACTIVE, JoinedAt = Now },
            new ClassMember { Id = 5, ClassId = 10, StudentId = 14, Status = ClassMemberStatus.ACTIVE, JoinedAt = Now },
            new ClassMember { Id = 6, ClassId = 22, StudentId = 14, Status = ClassMemberStatus.ACTIVE, JoinedAt = Now },
            new ClassMember { Id = 7, ClassId = 8, StudentId = 15, Status = ClassMemberStatus.ACTIVE, JoinedAt = Now },
            new ClassMember { Id = 8, ClassId = 20, StudentId = 15, Status = ClassMemberStatus.ACTIVE, JoinedAt = Now },
            new ClassMember { Id = 9, ClassId = 23, StudentId = 15, Status = ClassMemberStatus.ACTIVE, JoinedAt = Now },
            new ClassMember { Id = 10, ClassId = 4, StudentId = 3, Status = ClassMemberStatus.ACTIVE, JoinedAt = Now },
            new ClassMember { Id = 11, ClassId = 18, StudentId = 3, Status = ClassMemberStatus.ACTIVE, JoinedAt = Now },
            new ClassMember { Id = 12, ClassId = 11, StudentId = 14, Status = ClassMemberStatus.PENDING, JoinedAt = Now },
            new ClassMember { Id = 13, ClassId = 14, StudentId = 15, Status = ClassMemberStatus.ACTIVE, JoinedAt = Now }
        );
    }
}
