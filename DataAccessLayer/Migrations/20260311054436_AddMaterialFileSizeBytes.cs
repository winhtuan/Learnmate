using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialFileSizeBytes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "file_size_bytes",
                table: "materials",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "materials",
                keyColumn: "id",
                keyValue: 1L,
                column: "file_size_bytes",
                value: null);

            migrationBuilder.UpdateData(
                table: "materials",
                keyColumn: "id",
                keyValue: 2L,
                column: "file_size_bytes",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_size_bytes",
                table: "materials");
        }
    }
}
