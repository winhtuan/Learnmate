using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedPRNTotalSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 2L,
                column: "total_sessions",
                value: 24);

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 3L,
                column: "total_sessions",
                value: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 2L,
                column: "total_sessions",
                value: null);

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 3L,
                column: "total_sessions",
                value: null);
        }
    }
}
