using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddTutorOnboardingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "languages_spoken",
                table: "teacher_profiles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "teaching_philosophy",
                table: "teacher_profiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "years_of_experience",
                table: "teacher_profiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "teacher_documents",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 1L,
                columns: new[] { "languages_spoken", "teaching_philosophy", "years_of_experience" },
                values: new object[] { null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "languages_spoken",
                table: "teacher_profiles");

            migrationBuilder.DropColumn(
                name: "teaching_philosophy",
                table: "teacher_profiles");

            migrationBuilder.DropColumn(
                name: "years_of_experience",
                table: "teacher_profiles");

            migrationBuilder.DropColumn(
                name: "category",
                table: "teacher_documents");
        }
    }
}
