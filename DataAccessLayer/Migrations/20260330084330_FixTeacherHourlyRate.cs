using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class FixTeacherHourlyRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 1L,
                columns: new[] { "hourly_rate", "rating_avg", "total_rating_count" },
                values: new object[] { 35m, 3.8m, 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 1L,
                columns: new[] { "hourly_rate", "rating_avg", "total_rating_count" },
                values: new object[] { 200000m, 0m, 0 });
        }
    }
}
