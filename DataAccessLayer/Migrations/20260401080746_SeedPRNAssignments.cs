using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedPRNAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // New columns not covered by AddBookingPaymentVnPay
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

            // FK + index for class_id on tutor_booking_requests (column was added by AddBookingPaymentVnPay, FK/index were not)
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

            // ── Seed data ─────────────────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "assignments",
                columns: new[] { "id", "class_id", "created_at", "deleted_at", "description", "due_date", "file_url", "status", "teacher_id", "title", "updated_at" },
                values: new object[,]
                {
                    { 3L, 2L, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "Áp dụng mô hình MVVM vào ứng dụng MAUI: tạo ViewModel, sử dụng INotifyPropertyChanged và binding hai chiều cho form nhập liệu.", new DateTime(2026, 4, 10, 16, 59, 0, 0, DateTimeKind.Utc), null, "PUBLISHED", 2L, "Lab 2: MVVM & Data Binding", new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4L, 2L, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, "Trả lời các câu hỏi về XAML layout: StackLayout, Grid, FlexLayout, và các thuộc tính căn chỉnh trong .NET MAUI.", new DateTime(2026, 3, 20, 16, 59, 0, 0, DateTimeKind.Utc), null, "PUBLISHED", 2L, "Quiz 1: XAML & Layout Fundamentals", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 1L,
                column: "price",
                value: 1500000m);

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 2L,
                column: "price",
                value: 2000000m);

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 3L,
                column: "price",
                value: 2000000m);

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 4L,
                column: "price",
                value: 1500000m);

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 5L,
                column: "price",
                value: 1500000m);

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 6L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1500000m, new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), 32 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 7L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1500000m, new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), 32 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 8L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2500000m, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 48 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 9L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2000000m, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 48 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 10L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), 2000000m, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 12 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 11L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), 2000000m, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), 12 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 12L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1200000m, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), 16 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 13L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1200000m, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), 16 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 14L,
                columns: new[] { "end_date", "price", "start_date" },
                values: new object[] { new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1800000m, new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 15L,
                columns: new[] { "end_date", "price", "start_date" },
                values: new object[] { new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1800000m, new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 16L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1500000m, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 32 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 17L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1500000m, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 32 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 18L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1500000m, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 40 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 19L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1500000m, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 40 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 20L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 2500000m, new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 21L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1200000m, new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 22L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2027, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3000000m, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), 48 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 23L,
                columns: new[] { "end_date", "price", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2027, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1800000m, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), 48 });

            migrationBuilder.InsertData(
                table: "assignment_questions",
                columns: new[] { "id", "assignment_id", "content", "created_at", "order", "points", "type", "updated_at" },
                values: new object[,]
                {
                    { 4L, 3L, "Tạo ViewModel kế thừa INotifyPropertyChanged và bind vào View bằng two-way binding.", new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, 5m, "ESSAY", new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5L, 3L, "Viết Command (ICommand) để xử lý sự kiện button click trong ViewModel.", new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), 2, 5m, "ESSAY", new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6L, 4L, "So sánh StackLayout, Grid và FlexLayout trong MAUI. Khi nào nên dùng loại nào?", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, 3m, "MULTIPLE_CHOICE", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7L, 4L, "Thuộc tính nào dùng để căn giữa một phần tử theo chiều ngang trong Grid?", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2, 3m, "MULTIPLE_CHOICE", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8L, 4L, "Giải thích sự khác biệt giữa Margin và Padding trong XAML.", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, 4m, "ESSAY", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
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

            migrationBuilder.DeleteData(
                table: "assignment_questions",
                keyColumn: "id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "assignment_questions",
                keyColumn: "id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "assignment_questions",
                keyColumn: "id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "assignment_questions",
                keyColumn: "id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "assignment_questions",
                keyColumn: "id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "assignments",
                keyColumn: "id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "assignments",
                keyColumn: "id",
                keyValue: 4L);

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

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 6L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 7L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 8L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 9L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 10L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 11L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 12L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 13L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 14L,
                columns: new[] { "end_date", "start_date" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 15L,
                columns: new[] { "end_date", "start_date" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 16L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 17L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 18L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 19L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 20L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 21L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 22L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });

            migrationBuilder.UpdateData(
                table: "classes",
                keyColumn: "id",
                keyValue: 23L,
                columns: new[] { "end_date", "start_date", "total_sessions" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24 });
        }
    }
}
