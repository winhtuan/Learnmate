using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddTutorBookingRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tutor_booking_requests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    requested_start_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    requested_end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    result_class_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tutor_booking_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_tutor_booking_requests_classes_result_class_id",
                        column: x => x.result_class_id,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_tutor_booking_requests_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tutor_booking_requests_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tutor_booking_requests_result_class_id",
                table: "tutor_booking_requests",
                column: "result_class_id");

            migrationBuilder.CreateIndex(
                name: "ix_tutor_booking_requests_status",
                table: "tutor_booking_requests",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_tutor_booking_requests_student_id_teacher_id",
                table: "tutor_booking_requests",
                columns: new[] { "student_id", "teacher_id" });

            migrationBuilder.CreateIndex(
                name: "ix_tutor_booking_requests_teacher_id",
                table: "tutor_booking_requests",
                column: "teacher_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tutor_booking_requests");
        }
    }
}
