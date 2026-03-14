using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddClassThumbnailUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "thumbnail_url",
                table: "classes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true
            );

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 1L,
                column: "thumbnail_url",
                value: null
            );

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 2L,
                column: "thumbnail_url",
                value: null
            );

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 3L,
                column: "thumbnail_url",
                value: null
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "thumbnail_url", table: "classes");
        }
    }
}
