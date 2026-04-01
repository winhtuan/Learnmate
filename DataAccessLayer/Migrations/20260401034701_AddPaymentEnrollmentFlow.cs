using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentEnrollmentFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "failure_reason",
                table: "payments",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "method",
                table: "payments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "student_id",
                table: "invoices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "classes",
                type: "numeric(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "ix_tutor_booking_requests_class_id",
                table: "tutor_booking_requests",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_student_id",
                table: "invoices",
                column: "student_id");

            migrationBuilder.AddForeignKey(
                name: "fk_tutor_booking_requests_classes_class_id",
                table: "tutor_booking_requests",
                column: "class_id",
                principalTable: "classes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tutor_booking_requests_classes_class_id",
                table: "tutor_booking_requests");

            migrationBuilder.DropIndex(
                name: "ix_tutor_booking_requests_class_id",
                table: "tutor_booking_requests");

            migrationBuilder.DropIndex(
                name: "ix_invoices_student_id",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "failure_reason",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "method",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "student_id",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "price",
                table: "classes");
        }
    }
}
