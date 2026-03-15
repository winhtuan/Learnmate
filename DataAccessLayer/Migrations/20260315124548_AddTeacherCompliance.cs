using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherCompliance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "classes",
                keyColumn: "id",
                keyValue: 5L);

            migrationBuilder.AddColumn<string>(
                name: "admin_notes",
                table: "teacher_profiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "teacher_profiles",
                type: "text",
                nullable: false,
                defaultValue: "PENDING");

            migrationBuilder.AddColumn<DateTime>(
                name: "verified_at",
                table: "teacher_profiles",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "teacher_documents",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_profile_id = table.Column<long>(type: "bigint", nullable: false),
                    document_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    file_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    file_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teacher_documents", x => x.id);
                    table.ForeignKey(
                        name: "fk_teacher_documents_teacher_profiles_teacher_profile_id",
                        column: x => x.teacher_profile_id,
                        principalTable: "teacher_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "teacher_profiles",
                keyColumn: "id",
                keyValue: 1L,
                columns: new[] { "admin_notes", "verified_at" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "ix_teacher_documents_teacher_profile_id",
                table: "teacher_documents",
                column: "teacher_profile_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "teacher_documents");

            migrationBuilder.DropColumn(
                name: "admin_notes",
                table: "teacher_profiles");

            migrationBuilder.DropColumn(
                name: "status",
                table: "teacher_profiles");

            migrationBuilder.DropColumn(
                name: "verified_at",
                table: "teacher_profiles");

            migrationBuilder.InsertData(
                table: "classes",
                columns: new[] { "id", "created_at", "deleted_at", "description", "max_students", "name", "status", "subject", "teacher_id", "thumbnail_url", "updated_at" },
                values: new object[,]
                {
                    { 4L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chuyên đề Vật Lý chuyên sâu luyện thi Đại học.", 15, "Vật Lý 12 — Nâng cao", "ACTIVE", "Vật Lý", 2L, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lớp lấy lại gốc Đại số Toán 11.", 30, "Toán 11 — Đại số cơ bản", "ACTIVE", "Toán", 2L, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }
    }
}
