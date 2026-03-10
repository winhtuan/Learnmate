using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedStudent3ClassData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "study_streak_days",
                table: "student_profiles",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.InsertData(
                table: "classes",
                columns: new[]
                {
                    "id",
                    "created_at",
                    "deleted_at",
                    "description",
                    "max_students",
                    "name",
                    "subject",
                    "teacher_id",
                    "updated_at",
                },
                values: new object[,]
                {
                    {
                        2L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        null,
                        "Cross-Platform Mobile App Development With .NET MAUI",
                        30,
                        "PRN222",
                        "PRN222",
                        2L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    },
                    {
                        3L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        null,
                        "Unity Game Development — Scripting & Physics",
                        30,
                        "PRU213",
                        "PRU213",
                        2L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "notifications",
                columns: new[]
                {
                    "id",
                    "content",
                    "created_at",
                    "is_read",
                    "title",
                    "updated_at",
                    "user_id",
                },
                values: new object[,]
                {
                    {
                        1L,
                        "Bạn đã tham gia lớp Cross-Platform Mobile App Development thành công!",
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        true,
                        "Chào mừng đến PRN222",
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        3L,
                    },
                    {
                        2L,
                        "Bạn đã tham gia lớp Unity Game Development thành công!",
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        true,
                        "Chào mừng đến PRU213",
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        3L,
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "notifications",
                columns: new[] { "id", "content", "created_at", "title", "updated_at", "user_id" },
                values: new object[,]
                {
                    {
                        3L,
                        "Lab 1: MAUI Navigation đã được đăng. Hạn nộp: 16/03/2026.",
                        new DateTime(2026, 3, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                        "Bài tập mới — PRN222",
                        new DateTime(2026, 3, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                        3L,
                    },
                    {
                        4L,
                        "Lab 1: Unity Scene đã được đăng. Hạn nộp: 15/03/2026.",
                        new DateTime(2026, 3, 7, 2, 0, 0, 0, DateTimeKind.Utc),
                        "Bài tập mới — PRU213",
                        new DateTime(2026, 3, 7, 2, 0, 0, 0, DateTimeKind.Utc),
                        3L,
                    },
                    {
                        5L,
                        "PRN222: Thứ 2 & Thứ 4 lúc 07:30. PRU213: Thứ 3 & Thứ 5 lúc 13:00.",
                        new DateTime(2026, 3, 8, 3, 0, 0, 0, DateTimeKind.Utc),
                        "Lịch học tuần này",
                        new DateTime(2026, 3, 8, 3, 0, 0, 0, DateTimeKind.Utc),
                        3L,
                    },
                }
            );

            migrationBuilder.UpdateData(
                table: "student_profiles",
                keyColumn: "id",
                keyValue: 1L,
                columns: new[] { "full_name", "study_streak_days" },
                values: new object[] { "Nguyen Minh Tuan", 8 }
            );

            migrationBuilder.InsertData(
                table: "assignments",
                columns: new[]
                {
                    "id",
                    "class_id",
                    "created_at",
                    "deleted_at",
                    "description",
                    "due_date",
                    "status",
                    "teacher_id",
                    "title",
                    "updated_at",
                },
                values: new object[,]
                {
                    {
                        1L,
                        2L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        null,
                        "Xây dựng ứng dụng MAUI có ít nhất 3 màn hình với Shell Navigation và truyền dữ liệu giữa các trang.",
                        new DateTime(2026, 3, 16, 16, 59, 0, 0, DateTimeKind.Utc),
                        "PUBLISHED",
                        2L,
                        "Lab 1: MAUI Navigation",
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    },
                    {
                        2L,
                        3L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        null,
                        "Tạo một scene Unity có ánh sáng directional, mặt phẳng (Plane), và ít nhất 1 GameObject có Rigidbody và Collider.",
                        new DateTime(2026, 3, 15, 16, 59, 0, 0, DateTimeKind.Utc),
                        "PUBLISHED",
                        2L,
                        "Lab 1: Unity Scene",
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "class_members",
                columns: new[] { "id", "class_id", "joined_at", "status", "student_id" },
                values: new object[,]
                {
                    {
                        2L,
                        2L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        "ACTIVE",
                        3L,
                    },
                    {
                        3L,
                        3L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        "ACTIVE",
                        3L,
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "materials",
                columns: new[]
                {
                    "id",
                    "class_id",
                    "created_at",
                    "description",
                    "file_type",
                    "file_url",
                    "title",
                    "updated_at",
                    "uploaded_by",
                },
                values: new object[,]
                {
                    {
                        1L,
                        2L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Tổng quan về .NET MAUI, XAML cơ bản và cấu trúc project.",
                        "PDF",
                        "https://storage.learnmate.vn/materials/prn222-slide-01.pdf",
                        "Slide Buổi 1 — .NET MAUI Introduction",
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        2L,
                    },
                    {
                        2L,
                        3L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Hướng dẫn cài đặt Unity Hub, tạo project và làm quen với Editor.",
                        "PDF",
                        "https://storage.learnmate.vn/materials/pru213-slide-01.pdf",
                        "Slide Buổi 1 — Unity Editor Overview",
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        2L,
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "schedules",
                columns: new[]
                {
                    "id",
                    "class_id",
                    "created_at",
                    "end_time",
                    "start_time",
                    "title",
                    "type",
                    "updated_at",
                },
                values: new object[,]
                {
                    {
                        1L,
                        2L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 3, 9, 2, 30, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 3, 9, 0, 30, 0, 0, DateTimeKind.Utc),
                        "PRN222 — Buổi 1: Giới thiệu .NET MAUI & XAML",
                        "REGULAR",
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    },
                    {
                        2L,
                        2L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 3, 11, 2, 30, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 3, 11, 0, 30, 0, 0, DateTimeKind.Utc),
                        "PRN222 — Buổi 2: Data Binding & MVVM",
                        "REGULAR",
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    },
                    {
                        3L,
                        3L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 3, 10, 8, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 3, 10, 6, 0, 0, 0, DateTimeKind.Utc),
                        "PRU213 — Buổi 1: Unity Editor & Scene Setup",
                        "REGULAR",
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    },
                    {
                        4L,
                        3L,
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 3, 12, 8, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 3, 12, 6, 0, 0, 0, DateTimeKind.Utc),
                        "PRU213 — Buổi 2: Rigidbody Physics & Colliders",
                        "REGULAR",
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "assignments", keyColumn: "id", keyValue: 1L);

            migrationBuilder.DeleteData(table: "assignments", keyColumn: "id", keyValue: 2L);

            migrationBuilder.DeleteData(table: "class_members", keyColumn: "id", keyValue: 2L);

            migrationBuilder.DeleteData(table: "class_members", keyColumn: "id", keyValue: 3L);

            migrationBuilder.DeleteData(table: "materials", keyColumn: "id", keyValue: 1L);

            migrationBuilder.DeleteData(table: "materials", keyColumn: "id", keyValue: 2L);

            migrationBuilder.DeleteData(table: "notifications", keyColumn: "id", keyValue: 1L);

            migrationBuilder.DeleteData(table: "notifications", keyColumn: "id", keyValue: 2L);

            migrationBuilder.DeleteData(table: "notifications", keyColumn: "id", keyValue: 3L);

            migrationBuilder.DeleteData(table: "notifications", keyColumn: "id", keyValue: 4L);

            migrationBuilder.DeleteData(table: "notifications", keyColumn: "id", keyValue: 5L);

            migrationBuilder.DeleteData(table: "schedules", keyColumn: "id", keyValue: 1L);

            migrationBuilder.DeleteData(table: "schedules", keyColumn: "id", keyValue: 2L);

            migrationBuilder.DeleteData(table: "schedules", keyColumn: "id", keyValue: 3L);

            migrationBuilder.DeleteData(table: "schedules", keyColumn: "id", keyValue: 4L);

            migrationBuilder.DeleteData(table: "classes", keyColumn: "id", keyValue: 2L);

            migrationBuilder.DeleteData(table: "classes", keyColumn: "id", keyValue: 3L);

            migrationBuilder.DropColumn(name: "study_streak_days", table: "student_profiles");

            migrationBuilder.UpdateData(
                table: "student_profiles",
                keyColumn: "id",
                keyValue: 1L,
                column: "full_name",
                value: "Tran Thi B"
            );
        }
    }
}
