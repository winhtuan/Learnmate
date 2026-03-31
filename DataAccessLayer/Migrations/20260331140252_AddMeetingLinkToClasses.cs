using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingLinkToClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "meeting_link",
                table: "classes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 1L,
                column: "meeting_link",
                value: null);

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 2L,
                column: "meeting_link",
                value: null);

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 3L,
                column: "meeting_link",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "meeting_link",
                table: "classes");
        }
    }
}
