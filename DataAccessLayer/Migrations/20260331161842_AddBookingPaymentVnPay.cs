using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingPaymentVnPay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cancel_reason",
                table: "tutor_booking_requests",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "class_id",
                table: "tutor_booking_requests",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "payment_deadline",
                table: "tutor_booking_requests",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bank_code",
                table: "payments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "booking_id",
                table: "payments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "expired_at",
                table: "payments",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "vnp_transaction_no",
                table: "payments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "vnp_txn_ref",
                table: "payments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cancel_reason",
                table: "tutor_booking_requests");

            migrationBuilder.DropColumn(
                name: "class_id",
                table: "tutor_booking_requests");

            migrationBuilder.DropColumn(
                name: "payment_deadline",
                table: "tutor_booking_requests");

            migrationBuilder.DropColumn(
                name: "bank_code",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "booking_id",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "expired_at",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "vnp_transaction_no",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "vnp_txn_ref",
                table: "payments");
        }
    }
}
