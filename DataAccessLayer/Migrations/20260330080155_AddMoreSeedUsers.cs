using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar_url", "created_at", "deleted_at", "email", "is_active", "password_hash", "role", "updated_at" },
                values: new object[,]
                {
                    { 4L, "https://placehold.co/400/fce7f3/be185d?text=TM", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "tran.thi.mai@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5L, "https://placehold.co/400/dcfce7/16a34a?text=LD", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "le.van.duc@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6L, "https://placehold.co/400/fef9c3/ca8a04?text=PH", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "pham.thi.huong@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7L, "https://placehold.co/400/ede9fe/7c3aed?text=NB", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "nguyen.quoc.bao@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8L, "https://placehold.co/400/ffedd5/ea580c?text=HL", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "hoang.thi.lan@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9L, "https://placehold.co/400/cffafe/0891b2?text=VK", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "vu.minh.khoa@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10L, "https://placehold.co/400/fce7f3/db2777?text=DT", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "do.thi.thu@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 11L, "https://placehold.co/400/e0f2fe/0284c7?text=BL", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bui.van.long@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 12L, "https://placehold.co/400/f1f5f9/475569?text=NB", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "nguyen.thi.bich@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 13L, "https://placehold.co/400/fef2f2/dc2626?text=TN", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "trinh.van.nam@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 14L, "https://placehold.co/400/e0e7ff/4f46e5?text=S2", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "student2@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "STUDENT", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 15L, "https://placehold.co/400/ecfdf5/059669?text=S3", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "student3@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "STUDENT", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "student_profiles",
                columns: new[] { "id", "created_at", "date_of_birth", "full_name", "grade_level", "parent_contact", "study_streak_days", "updated_at", "user_id" },
                values: new object[,]
                {
                    { 2L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Le Thi Hoa", "11", null, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14L },
                    { 3L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Pham Van Kien", "10", null, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15L }
                });

            migrationBuilder.InsertData(
                table: "teacher_profiles",
                columns: new[] { "id", "avatar_url", "bank_account", "bio", "created_at", "full_name", "hourly_rate", "rating_avg", "subjects", "total_rating_count", "updated_at", "user_id" },
                values: new object[,]
                {
                    { 2L, null, null, "Giáo viên Toán & Vật Lý với 7 năm kinh nghiệm luyện thi đại học. Hơn 200 học sinh đã đỗ các trường top.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tran Thi Mai", 28m, 4.5m, "Mathematics,Science", 42, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4L },
                    { 3L, null, null, "Thạc sĩ Hóa học, chuyên ôn luyện Hóa & Sinh cho học sinh THPT. Phương pháp dạy trực quan, dễ hiểu.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Le Van Duc", 35m, 4.2m, "Science", 28, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5L },
                    { 4L, null, null, "IELTS 8.0, 10 năm dạy Tiếng Anh giao tiếp và luyện thi IELTS/TOEIC. Cam kết đầu ra rõ ràng.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pham Thi Huong", 45m, 4.8m, "English", 105, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6L },
                    { 5L, null, null, "Kỹ sư phần mềm tại FPT Software, 5 năm dạy lập trình Python, C#, và Web Development cho mọi trình độ.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nguyen Quoc Bao", 55m, 4.6m, "Coding", 67, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7L },
                    { 6L, null, null, "Giáo viên Ngữ Văn & Lịch Sử THPT Quốc Gia. Chuyên luyện đề thi và viết văn nghị luận xã hội.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hoang Thi Lan", 22m, 4.0m, "English", 19, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8L },
                    { 7L, null, null, "Tiến sĩ Toán ứng dụng, cựu giảng viên ĐH Bách Khoa. Dạy Toán cao cấp, Thống kê và Tin học.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vu Minh Khoa", 40m, 4.7m, "Mathematics,Coding", 83, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9L },
                    { 8L, null, null, "Tốt nghiệp ĐH Ngoại Ngữ, dạy Tiếng Anh và Tiếng Pháp. 8 năm kinh nghiệm, lớp học tương tác cao.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Do Thi Thu", 38m, 4.3m, "English,Languages", 51, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10L },
                    { 9L, null, null, "Cựu học sinh Chuyên Lý ĐH Khoa Học Tự Nhiên. Dạy Vật Lý và Hóa học theo hướng tư duy phân tích.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bui Van Long", 30m, 4.1m, "Science,Mathematics", 33, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11L },
                    { 10L, null, null, "Giáo viên Toán chuyên, huy chương Bạc Olympic Toán quốc gia. Đam mê giúp học sinh yêu thích Toán học.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nguyen Thi Bich", 25m, 4.9m, "Mathematics", 134, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12L },
                    { 11L, null, null, "Senior Developer 8 năm kinh nghiệm, chuyên dạy Lập trình và Toán rời rạc cho sinh viên CNTT.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Trinh Van Nam", 65m, 4.4m, "Coding,Mathematics", 58, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "student_profiles",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "student_profiles",
                keyColumn: "id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 15L);
        }
    }
}
