using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedTutorClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "classes",
                columns: new[] { "id", "created_at", "deleted_at", "description", "max_students", "meeting_link", "name", "subject", "teacher_id", "thumbnail_url", "updated_at" },
                values: new object[,]
                {
                    { 4L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn luyện toàn bộ chương trình Toán 12: Giới hạn, Đạo hàm, Tích phân, Hình học không gian. Tập trung vào dạng đề thi THPT QG.", 20, null, "Toán 12 — Luyện thi THPT Quốc Gia", "Mathematics", 4L, "https://placehold.co/400/fce7f3/be185d?text=T12", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chuyên sâu Điện học và Từ trường lớp 11: điện tích, tụ điện, định luật Ohm, từ trường, cảm ứng điện từ.", 15, null, "Vật Lý 11 — Điện và Từ Trường", "Science", 4L, "https://placehold.co/400/fce7f3/be185d?text=VL11", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hệ thống hóa toàn bộ Hóa 12: Polymer, Este-Lipid, Amin-Aminoaxit, Kim loại kiềm, Kim loại chuyển tiếp. Luyện đề THPT.", 18, null, "Hóa Học 12 — Hữu Cơ & Vô Cơ", "Science", 5L, "https://placehold.co/400/dcfce7/16a34a?text=HH12", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sinh lý thực vật: trao đổi chất, quang hợp, hô hấp. Sinh lý động vật: tuần hoàn, hô hấp, bài tiết, thần kinh.", 15, null, "Sinh Học 11 — Sinh Lý Thực Vật & Động Vật", "Science", 5L, "https://placehold.co/400/dcfce7/16a34a?text=SH11", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Luyện thi IELTS toàn diện 4 kỹ năng: Listening, Reading, Writing Task 1&2, Speaking. Cam kết đầu ra Band 6.5 sau 3 tháng.", 12, null, "IELTS Preparation — Band 6.5+", "English", 6L, "https://placehold.co/400/fef9c3/ca8a04?text=IELTS", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Phát triển kỹ năng giao tiếp tiếng Anh tự nhiên: phát âm, ngữ điệu, từ vựng thực tế. Phù hợp trình độ A2-B1.", 15, null, "English Communication — Intermediate", "English", 6L, "https://placehold.co/400/fef9c3/ca8a04?text=ENG", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Từ cú pháp Python đến OOP, xử lý file, API, và Data Science cơ bản với Pandas/NumPy. Thực hành dự án thực tế.", 20, null, "Python Cơ Bản đến Nâng Cao", "Coding", 7L, "https://placehold.co/400/ede9fe/7c3aed?text=PY", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 11L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xây dựng Web API RESTful với ASP.NET Core 8, Entity Framework Core, JWT Auth, và deploy lên Azure. Thực hành project cuối khóa.", 15, null, "Web Development với ASP.NET Core", "Coding", 7L, "https://placehold.co/400/ede9fe/7c3aed?text=NET", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 12L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Luyện viết văn nghị luận xã hội và phân tích tác phẩm văn học lớp 12. Bí quyết đạt điểm cao môn Văn THPT QG.", 20, null, "Ngữ Văn 12 — Nghị Luận Xã Hội & Văn Học", "English", 8L, "https://placehold.co/400/ffedd5/ea580c?text=VAN", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 13L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hệ thống toàn bộ Lịch Sử 12: Lịch sử thế giới, Lịch sử Việt Nam 1919-2000. Luyện câu hỏi tư duy và ghi nhớ nhanh.", 18, null, "Lịch Sử 12 — Ôn thi THPT Quốc Gia", "English", 8L, "https://placehold.co/400/ffedd5/ea580c?text=LS", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 14L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Giải tích hàm một biến và nhiều biến, Đại số tuyến tính, Phương trình vi phân. Phù hợp sinh viên ĐH Kỹ thuật.", 25, null, "Toán Cao Cấp A1 & A2 cho Sinh viên", "Mathematics", 9L, "https://placehold.co/400/cffafe/0891b2?text=TCC", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 15L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xác suất, Phân phối xác suất, Kiểm định giả thuyết, Hồi quy tuyến tính. Ứng dụng trong Data Science và Kinh tế.", 20, null, "Thống Kê Xác Suất & Ứng Dụng", "Mathematics", 9L, "https://placehold.co/400/cffafe/0891b2?text=TK", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 16L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Khóa học tiếng Anh giao tiếp dành cho người mới bắt đầu. Xây dựng vốn từ vựng, phát âm chuẩn và phản xạ hội thoại cơ bản.", 12, null, "Tiếng Anh Giao Tiếp — Beginner A1-A2", "English", 10L, "https://placehold.co/400/fce7f3/db2777?text=ENG", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 17L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tiếng Pháp từ đầu: ngữ âm, ngữ pháp cơ bản, hội thoại thực tế. Chuẩn bị cho kỳ thi DELF A1 và A2.", 10, null, "Tiếng Pháp Cơ Bản A1-A2 (DELF)", "English", 10L, "https://placehold.co/400/fce7f3/db2777?text=FR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 18L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dao động, Sóng, Điện xoay chiều, Lượng tử ánh sáng, Hạt nhân nguyên tử. Luyện đề THPT QG theo cấu trúc mới.", 20, null, "Vật Lý 12 — Luyện thi THPT Quốc Gia", "Science", 11L, "https://placehold.co/400/e0f2fe/0284c7?text=VL12", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 19L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đại cương hóa hữu cơ, Hydrocarbon, Dẫn xuất Halogen, Ancol-Phenol-Ete, Andehit-Axit Cacboxylic. Bài tập từ cơ bản đến nâng cao.", 18, null, "Hóa Học 11 — Hóa Hữu Cơ Cơ Bản", "Science", 11L, "https://placehold.co/400/e0f2fe/0284c7?text=HH11", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 20L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dành cho học sinh chuyên Toán và HSG: Số học, Đại số, Hình học Euclidean, Tổ hợp. Luyện đề Olympic cấp tỉnh và quốc gia.", 10, null, "Toán Chuyên — Luyện thi Olympic & Đại học", "Mathematics", 12L, "https://placehold.co/400/f1f5f9/475569?text=OLP", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 21L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xây dựng nền tảng Toán lớp 10 vững chắc: Mệnh đề-Tập hợp, Hàm số, Phương trình-Bất phương trình, Hình học phẳng, Lượng giác.", 20, null, "Toán 10 — Đại Số, Hình Học & Lượng Giác", "Mathematics", 12L, "https://placehold.co/400/f1f5f9/475569?text=T10", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 22L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "C# từ cơ bản đến nâng cao, ASP.NET Core MVC, EF Core, SQL Server, React + Blazor. Xây dựng portfolio project thực tế.", 15, null, "Lập Trình C# — .NET Full Stack Developer", "Coding", 13L, "https://placehold.co/400/fef2f2/dc2626?text=CS", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 23L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Logic mệnh đề, Lý thuyết tập hợp, Quan hệ, Hàm, Lý thuyết đồ thị, Đếm tổ hợp, Xác suất rời rạc. Ứng dụng trong Khoa học máy tính.", 20, null, "Toán Rời Rạc cho Sinh viên CNTT", "Mathematics", 13L, "https://placehold.co/400/fef2f2/dc2626?text=TRR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "class_members",
                columns: new[] { "id", "class_id", "joined_at", "status", "student_id" },
                values: new object[,]
                {
                    { 4L, 8L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ACTIVE", 14L },
                    { 5L, 10L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ACTIVE", 14L },
                    { 6L, 22L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ACTIVE", 14L },
                    { 7L, 8L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ACTIVE", 15L },
                    { 8L, 20L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ACTIVE", 15L },
                    { 9L, 23L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ACTIVE", 15L },
                    { 10L, 4L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ACTIVE", 3L },
                    { 11L, 18L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ACTIVE", 3L }
                });

            migrationBuilder.InsertData(
                table: "class_members",
                columns: new[] { "id", "class_id", "joined_at", "student_id" },
                values: new object[] { 12L, 11L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14L });

            migrationBuilder.InsertData(
                table: "class_members",
                columns: new[] { "id", "class_id", "joined_at", "status", "student_id" },
                values: new object[] { 13L, 14L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ACTIVE", 15L });

            migrationBuilder.InsertData(
                table: "schedules",
                columns: new[] { "id", "class_id", "created_at", "end_time", "start_time", "title", "type", "updated_at" },
                values: new object[,]
                {
                    { 5L, 4L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 11, 0, 0, 0, DateTimeKind.Utc), "Toán 12 — Buổi 1: Giới hạn & Liên tục", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6L, 4L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 11, 0, 0, 0, DateTimeKind.Utc), "Toán 12 — Buổi 2: Đạo hàm & Ứng dụng", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7L, 5L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 11, 0, 0, 0, DateTimeKind.Utc), "Vật Lý 11 — Buổi 1: Tĩnh điện học", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8L, 5L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 11, 0, 0, 0, DateTimeKind.Utc), "Vật Lý 11 — Buổi 2: Điện trường & Tụ điện", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9L, 6L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Hóa 12 — Buổi 1: Este và Lipit", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10L, 6L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Hóa 12 — Buổi 2: Cacbohidrat", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 11L, 7L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Sinh 11 — Buổi 1: Trao đổi nước ở thực vật", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 12L, 7L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Utc), "Sinh 11 — Buổi 2: Quang hợp", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 13L, 8L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 12, 0, 0, 0, DateTimeKind.Utc), "IELTS — Buổi 1: Reading Strategies", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 14L, 8L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 12, 0, 0, 0, DateTimeKind.Utc), "IELTS — Buổi 2: Writing Task 2 Structure", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 15L, 9L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 12, 0, 0, 0, DateTimeKind.Utc), "English Comm — Buổi 1: Greetings & Small Talk", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 16L, 9L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 12, 0, 0, 0, DateTimeKind.Utc), "English Comm — Buổi 2: Describing People", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 17L, 10L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 11, 0, 0, 0, DateTimeKind.Utc), "Python — Buổi 1: Biến, Kiểu dữ liệu, Vòng lặp", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 18L, 10L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 11, 0, 0, 0, DateTimeKind.Utc), "Python — Buổi 2: Hàm & Module", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 19L, 11L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 11, 0, 0, 0, DateTimeKind.Utc), "ASP.NET — Buổi 1: Cấu trúc project & DI", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 20L, 11L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 13, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 13, 11, 0, 0, 0, DateTimeKind.Utc), "ASP.NET — Buổi 2: RESTful API & Routing", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 21L, 12L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 14, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 14, 1, 0, 0, 0, DateTimeKind.Utc), "Ngữ Văn — Buổi 1: Nghị luận xã hội", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 22L, 12L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 1, 0, 0, 0, DateTimeKind.Utc), "Ngữ Văn — Buổi 2: Phân tích thơ Tây Tiến", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 23L, 13L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 14, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 14, 7, 0, 0, 0, DateTimeKind.Utc), "Lịch Sử — Buổi 1: Liên Xô & Đông Âu 1945-1991", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 24L, 13L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 7, 0, 0, 0, DateTimeKind.Utc), "Lịch Sử — Buổi 2: Cách mạng Việt Nam 1930-1945", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 25L, 14L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 12, 0, 0, 0, DateTimeKind.Utc), "Toán CC — Buổi 1: Giới hạn & Đạo hàm hàm nhiều biến", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 26L, 14L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 12, 0, 0, 0, DateTimeKind.Utc), "Toán CC — Buổi 2: Tích phân bội", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 27L, 15L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 12, 0, 0, 0, DateTimeKind.Utc), "Thống Kê — Buổi 1: Biến ngẫu nhiên & Phân phối", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 28L, 15L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 12, 0, 0, 0, DateTimeKind.Utc), "Thống Kê — Buổi 2: Ước lượng tham số", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 29L, 16L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 4, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 2, 0, 0, 0, DateTimeKind.Utc), "Tiếng Anh — Buổi 1: Alphabet & Phonics", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 30L, 16L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 4, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 2, 0, 0, 0, DateTimeKind.Utc), "Tiếng Anh — Buổi 2: To be & Simple Present", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 31L, 17L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 4, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 2, 0, 0, 0, DateTimeKind.Utc), "Tiếng Pháp — Buổi 1: Bảng chữ cái & Ngữ âm", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 32L, 17L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 4, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 2, 0, 0, 0, DateTimeKind.Utc), "Tiếng Pháp — Buổi 2: Être & Avoir, Giới từ", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 33L, 18L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 10, 0, 0, 0, DateTimeKind.Utc), "Vật Lý 12 — Buổi 1: Dao động điều hoà", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 34L, 18L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 10, 0, 0, 0, DateTimeKind.Utc), "Vật Lý 12 — Buổi 2: Sóng cơ học", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 35L, 19L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 10, 0, 0, 0, DateTimeKind.Utc), "Hóa 11 — Buổi 1: Đại cương Hóa hữu cơ", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 36L, 19L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 10, 0, 0, 0, DateTimeKind.Utc), "Hóa 11 — Buổi 2: Ankan & Anken", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 37L, 20L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 14, 4, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 14, 2, 0, 0, 0, DateTimeKind.Utc), "Toán Olympic — Buổi 1: Bất đẳng thức Cauchy-Schwarz", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 38L, 20L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 4, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 2, 0, 0, 0, DateTimeKind.Utc), "Toán Olympic — Buổi 2: Lý thuyết số cơ bản", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 39L, 21L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 10, 0, 0, 0, DateTimeKind.Utc), "Toán 10 — Buổi 1: Mệnh đề & Tập hợp", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 40L, 21L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 10, 0, 0, 0, DateTimeKind.Utc), "Toán 10 — Buổi 2: Hàm số & Đồ thị", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 41L, 22L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 12, 0, 0, 0, DateTimeKind.Utc), "C# .NET — Buổi 1: OOP & SOLID Principles", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 42L, 22L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 12, 0, 0, 0, DateTimeKind.Utc), "C# .NET — Buổi 2: Async/Await & LINQ", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 43L, 23L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 12, 0, 0, 0, DateTimeKind.Utc), "Toán Rời Rạc — Buổi 1: Logic & Tập hợp", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 44L, 23L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 13, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 13, 12, 0, 0, 0, DateTimeKind.Utc), "Toán Rời Rạc — Buổi 2: Lý thuyết đồ thị", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "class_members",
                keyColumn: "id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "class_members",
                keyColumn: "id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "class_members",
                keyColumn: "id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "class_members",
                keyColumn: "id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "class_members",
                keyColumn: "id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "class_members",
                keyColumn: "id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "class_members",
                keyColumn: "id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "class_members",
                keyColumn: "id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "class_members",
                keyColumn: "id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "class_members",
                keyColumn: "id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 34L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 37L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 38L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 39L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 40L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 41L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 42L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 43L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 44L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 23L);
        }
    }
}
