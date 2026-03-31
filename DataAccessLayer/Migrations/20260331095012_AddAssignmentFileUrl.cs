using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentFileUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "file_url",
                table: "assignments",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "assignments",
                keyColumn: "id",
                keyValue: 1L,
                column: "file_url",
                value: null);

            migrationBuilder.UpdateData(
                table: "assignments",
                keyColumn: "id",
                keyValue: 2L,
                column: "file_url",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_url",
                table: "assignments");
        }
    }
}
