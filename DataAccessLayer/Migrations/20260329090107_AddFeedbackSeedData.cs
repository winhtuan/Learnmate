using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 5L);

            migrationBuilder.InsertData(
                table: "assignment_questions",
                columns: new[] { "id", "assignment_id", "content", "created_at", "order", "points", "type", "updated_at" },
                values: new object[,]
                {
                    { 1L, 1L, "Triển khai Shell Navigation giữa ít nhất 3 màn hình.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 5m, "ESSAY", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, 1L, "Truyền dữ liệu giữa các trang bằng QueryProperty hoặc constructor injection.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 5m, "ESSAY", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3L, 2L, "Tạo scene Unity với đầy đủ ánh sáng, Plane, Rigidbody và Collider.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 10m, "ESSAY", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "submissions",
                columns: new[] { "id", "assignment_id", "created_at", "deleted_at", "file_url", "score", "status", "student_id", "submitted_at", "updated_at" },
                values: new object[,]
                {
                    { 1L, 1L, new DateTime(2026, 3, 14, 10, 0, 0, 0, DateTimeKind.Utc), null, null, 8.5m, "GRADED", 3L, new DateTime(2026, 3, 14, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, 2L, new DateTime(2026, 3, 14, 15, 30, 0, 0, DateTimeKind.Utc), null, null, null, "SUBMITTED", 3L, new DateTime(2026, 3, 14, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 14, 15, 30, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "feedbacks",
                columns: new[] { "id", "comment", "created_at", "score", "submission_id", "updated_at" },
                values: new object[] { 1L, "Bài làm tốt! Navigation giữa các màn hình hoạt động đúng yêu cầu. Tuy nhiên, cần cải thiện phần truyền dữ liệu — nên dùng QueryProperty thay vì singleton. Tiếp tục cố gắng nhé!", new DateTime(2026, 3, 15, 8, 0, 0, 0, DateTimeKind.Utc), 8.5m, 1L, new DateTime(2026, 3, 15, 8, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "assignment_questions",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "assignment_questions",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "assignment_questions",
                keyColumn: "id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "feedbacks",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "submissions",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "submissions",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.InsertData(
                table: "classes",
                columns: new[] { "id", "created_at", "deleted_at", "description", "max_students", "name", "status", "subject", "teacher_id", "thumbnail_url", "updated_at" },
                values: new object[,]
                {
                    { 4L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chuyên đề Vật Lý chuyên sâu luyện thi Đại học.", 15, "Vật Lý 12 — Nâng cao", "ACTIVE", "Vật Lý", 2L, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lớp lấy lại gốc Đại số Toán 11.", 30, "Toán 11 — Đại số cơ bản", "ACTIVE", "Toán", 2L, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }
    }
}
