using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddClassThumbnailDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 1L,
                column: "thumbnail_url",
                value: "https://placehold.co/400?text=Course"
            );

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 2L,
                column: "thumbnail_url",
                value: "https://placehold.co/400?text=Course"
            );

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 3L,
                column: "thumbnail_url",
                value: "https://placehold.co/400?text=Course"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
