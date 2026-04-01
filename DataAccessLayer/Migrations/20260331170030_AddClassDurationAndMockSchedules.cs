using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddClassDurationAndMockSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "end_date",
                table: "classes",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "start_date",
                table: "classes",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total_sessions",
                table: "classes",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 1L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 2L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 3L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 4L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 5L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 6L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 7L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 8L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 9L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 10L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 11L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 12L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 13L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 14L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 15L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 16L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 17L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 18L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 19L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 20L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 21L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 22L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 23L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.InsertData(
                table: "schedules",
                columns: new[] { "id", "class_id", "created_at", "end_time", "start_time", "title", "type", "updated_at" },
                values: new object[,]
                {
                    { 101L, 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 2, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 0, 30, 0, 0, DateTimeKind.Utc), "Buổi 1 sáng", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 102L, 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 5, 0, 0, 0, DateTimeKind.Utc), "Buổi 2 trưa", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 101L);

            migrationBuilder.DeleteData(
                table: "schedules",
                keyColumn: "id",
                keyValue: 102L);

            migrationBuilder.DropColumn(
                name: "end_date",
                table: "classes");

            migrationBuilder.DropColumn(
                name: "start_date",
                table: "classes");

            migrationBuilder.DropColumn(
                name: "total_sessions",
                table: "classes");
        }
    }
}
