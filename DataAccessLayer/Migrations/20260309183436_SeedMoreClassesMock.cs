using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreClassesMock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "classes",
                columns: new[] { "id", "created_at", "deleted_at", "description", "max_students", "name", "subject", "teacher_id", "updated_at" },
                values: new object[,]
                {
                    { 4L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chuyên đề Vật Lý chuyên sâu luyện thi Đại học.", 15, "Vật Lý 12 — Nâng cao", "Vật Lý", 2L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lớp lấy lại gốc Đại số Toán 11.", 30, "Toán 11 — Đại số cơ bản", "Toán", 2L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 5L);
        }
    }
}
