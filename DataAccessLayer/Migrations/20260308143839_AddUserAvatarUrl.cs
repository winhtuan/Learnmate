using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAvatarUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                table: "users",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true
            );

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1L,
                column: "avatar_url",
                value: "https://placehold.co/400?text=Admin"
            );

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2L,
                column: "avatar_url",
                value: "https://placehold.co/400?text=Teacher"
            );

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3L,
                column: "avatar_url",
                value: "https://placehold.co/400?text=Student"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "avatar_url", table: "users");
        }
    }
}
