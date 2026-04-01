using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedDurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 6L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), 32 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 7L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), 32 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 8L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 48 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 9L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 48 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 10L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 12 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 11L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 12 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 12L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), 16 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 13L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), 16 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 14L,
                columns: new[] { "end_date", "start_date" },
                values: new object[] { new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 15L,
                columns: new[] { "end_date", "start_date" },
                values: new object[] { new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 16L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 32 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 17L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 32 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 18L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 40 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 19L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 40 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 20L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 21L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 22L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2027, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), 48 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 23L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2027, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), 48 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "end_date", "start_date" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 15L,
                columns: new[] { "end_date", "start_date" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

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
        }
    }
}
