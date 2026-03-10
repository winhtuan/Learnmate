using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudentAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "student_profiles",
                keyColumn: "id",
                keyValue: 1L,
                column: "avatar_url",
                value: "https://placehold.co/400?text=avatar"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "student_profiles",
                keyColumn: "id",
                keyValue: 1L,
                column: "avatar_url",
                value: "https://api.dicebear.com/9.x/avataaars/svg?seed=NguyenMinhTuan&backgroundColor=b6e3f4&clothesColor=3c4f5c&hairColor=2c1b18&facialHairType=Blank&accessories=prescription02&clothesType=hoodie&eyeType=Happy&eyebrowType=Default&mouthType=Smile&skinColor=Light"
            );
        }
    }
}
