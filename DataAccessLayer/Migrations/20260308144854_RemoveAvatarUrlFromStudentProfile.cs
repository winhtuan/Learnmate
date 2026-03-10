using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAvatarUrlFromStudentProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "avatar_url", table: "student_profiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                table: "student_profiles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true
            );

            migrationBuilder.UpdateData(
                table: "student_profiles",
                keyColumn: "id",
                keyValue: 1L,
                column: "avatar_url",
                value: "https://placehold.co/400?text=avatar"
            );
        }
    }
}
