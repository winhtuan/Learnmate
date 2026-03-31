using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false),
                    format = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    requested_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    file_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reports", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    avatar_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    subject = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "ACTIVE"),
                    max_students = table.Column<int>(type: "integer", nullable: false, defaultValue: 30),
                    thumbnail_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    meeting_link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_classes", x => x.id);
                    table.CheckConstraint("ck_classes_max_students", "max_students > 0");
                    table.ForeignKey(
                        name: "fk_classes_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "conversations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    participant_a_id = table.Column<long>(type: "bigint", nullable: false),
                    participant_b_id = table.Column<long>(type: "bigint", nullable: false),
                    last_message_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_conversations", x => x.id);
                    table.ForeignKey(
                        name: "fk_conversations_users_participant_a_id",
                        column: x => x.participant_a_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_conversations_users_participant_b_id",
                        column: x => x.participant_b_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "otp_verifications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    expired_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    attempt_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_otp_verifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_otp_verifications_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_profiles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    grade_level = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    parent_contact = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    study_streak_days = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_student_profiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_student_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teacher_profiles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    avatar_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    bio = table.Column<string>(type: "text", nullable: true),
                    subjects = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    hourly_rate = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    languages_spoken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    years_of_experience = table.Column<int>(type: "integer", nullable: true),
                    teaching_philosophy = table.Column<string>(type: "text", nullable: true),
                    rating_avg = table.Column<decimal>(type: "numeric(5,2)", nullable: false, defaultValue: 0m),
                    total_rating_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    bank_account = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "PENDING"),
                    admin_notes = table.Column<string>(type: "text", nullable: true),
                    verified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teacher_profiles", x => x.id);
                    table.CheckConstraint("ck_teacher_profiles_hourly_rate", "hourly_rate > 0");
                    table.CheckConstraint("ck_teacher_profiles_rating_avg", "rating_avg BETWEEN 0 AND 5");
                    table.ForeignKey(
                        name: "fk_teacher_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assignments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    class_id = table.Column<long>(type: "bigint", nullable: false),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "DRAFT"),
                    due_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    file_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_assignments_classes_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_assignments_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "class_members",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    class_id = table.Column<long>(type: "bigint", nullable: false),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "PENDING"),
                    joined_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_class_members", x => x.id);
                    table.ForeignKey(
                        name: "fk_class_members_classes_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_class_members_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    class_id = table.Column<long>(type: "bigint", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "PENDING"),
                    period_start = table.Column<DateOnly>(type: "date", nullable: false),
                    period_end = table.Column<DateOnly>(type: "date", nullable: false),
                    issued_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    paid_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoices", x => x.id);
                    table.CheckConstraint("ck_invoices_period", "period_start < period_end");
                    table.CheckConstraint("ck_invoices_total_amount", "total_amount > 0");
                    table.ForeignKey(
                        name: "fk_invoices_classes_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_invoices_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    class_id = table.Column<long>(type: "bigint", nullable: false),
                    uploaded_by = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    file_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    file_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    file_size_bytes = table.Column<long>(type: "bigint", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "ACTIVE"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_materials", x => x.id);
                    table.ForeignKey(
                        name: "fk_materials_classes_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_materials_users_uploaded_by",
                        column: x => x.uploaded_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "schedules",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    class_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "SCHEDULED"),
                    is_trial = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_schedules", x => x.id);
                    table.CheckConstraint("ck_schedules_time", "end_time > start_time");
                    table.ForeignKey(
                        name: "fk_schedules_classes_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teacher_ratings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    class_id = table.Column<long>(type: "bigint", nullable: false),
                    rating = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teacher_ratings", x => x.id);
                    table.CheckConstraint("ck_teacher_ratings_rating", "rating BETWEEN 1.0 AND 5.0");
                    table.ForeignKey(
                        name: "fk_teacher_ratings_classes_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teacher_ratings_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_teacher_ratings_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    conversation_id = table.Column<long>(type: "bigint", nullable: false),
                    sender_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    read_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_messages_conversations_conversation_id",
                        column: x => x.conversation_id,
                        principalTable: "conversations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_messages_users_sender_id",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "assignment_questions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    assignment_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    points = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assignment_questions", x => x.id);
                    table.CheckConstraint("ck_assignment_questions_order", "\"order\" > 0");
                    table.CheckConstraint("ck_assignment_questions_points", "points > 0");
                    table.ForeignKey(
                        name: "fk_assignment_questions_assignments_assignment_id",
                        column: x => x.assignment_id,
                        principalTable: "assignments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "submissions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    assignment_id = table.Column<long>(type: "bigint", nullable: false),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "DRAFT"),
                    score = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    submitted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    file_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_submissions", x => x.id);
                    table.CheckConstraint("ck_submissions_score", "score IS NULL OR score >= 0");
                    table.ForeignKey(
                        name: "fk_submissions_assignments_assignment_id",
                        column: x => x.assignment_id,
                        principalTable: "assignments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_submissions_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    class_id = table.Column<long>(type: "bigint", nullable: false),
                    invoice_id = table.Column<long>(type: "bigint", nullable: true),
                    amount = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "PENDING"),
                    paid_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payments", x => x.id);
                    table.CheckConstraint("ck_payments_amount", "amount > 0");
                    table.ForeignKey(
                        name: "fk_payments_classes_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_payments_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_payments_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "attendances",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    schedule_id = table.Column<long>(type: "bigint", nullable: false),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    is_present = table.Column<bool>(type: "boolean", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attendances", x => x.id);
                    table.ForeignKey(
                        name: "fk_attendances_schedules_schedule_id",
                        column: x => x.schedule_id,
                        principalTable: "schedules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_attendances_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "video_sessions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    schedule_id = table.Column<long>(type: "bigint", nullable: false),
                    provider = table.Column<string>(type: "text", nullable: false),
                    meeting_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    meeting_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "WAITING"),
                    started_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ended_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_video_sessions", x => x.id);
                    table.ForeignKey(
                        name: "fk_video_sessions_schedules_schedule_id",
                        column: x => x.schedule_id,
                        principalTable: "schedules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assignment_options",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    is_correct = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assignment_options", x => x.id);
                    table.CheckConstraint("ck_assignment_options_order", "\"order\" > 0");
                    table.ForeignKey(
                        name: "fk_assignment_options_assignment_questions_question_id",
                        column: x => x.question_id,
                        principalTable: "assignment_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "feedbacks",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    submission_id = table.Column<long>(type: "bigint", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    score = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feedbacks", x => x.id);
                    table.CheckConstraint("ck_feedbacks_score", "score >= 0");
                    table.ForeignKey(
                        name: "fk_feedbacks_submissions_submission_id",
                        column: x => x.submission_id,
                        principalTable: "submissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "submission_answers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    submission_id = table.Column<long>(type: "bigint", nullable: false),
                    question_id = table.Column<long>(type: "bigint", nullable: false),
                    answer_text = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_submission_answers", x => x.id);
                    table.ForeignKey(
                        name: "fk_submission_answers_assignment_questions_question_id",
                        column: x => x.question_id,
                        principalTable: "assignment_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_submission_answers_submissions_submission_id",
                        column: x => x.submission_id,
                        principalTable: "submissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "submission_answer_options",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    submission_answer_id = table.Column<long>(type: "bigint", nullable: false),
                    option_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_submission_answer_options", x => x.id);
                    table.ForeignKey(
                        name: "fk_submission_answer_options_assignment_options_option_id",
                        column: x => x.option_id,
                        principalTable: "assignment_options",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_submission_answer_options_submission_answers_submission_ans",
                        column: x => x.submission_answer_id,
                        principalTable: "submission_answers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar_url", "created_at", "deleted_at", "email", "is_active", "password_hash", "role", "updated_at" },
                values: new object[,]
                {
                    { 1L, "https://placehold.co/400?text=Admin", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "admin@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "ADMIN", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, "https://placehold.co/400?text=Teacher", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "teacher@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3L, "https://placehold.co/400?text=Student", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "student@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "STUDENT", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4L, "https://placehold.co/400/fce7f3/be185d?text=TM", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "tran.thi.mai@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5L, "https://placehold.co/400/dcfce7/16a34a?text=LD", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "le.van.duc@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6L, "https://placehold.co/400/fef9c3/ca8a04?text=PH", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "pham.thi.huong@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7L, "https://placehold.co/400/ede9fe/7c3aed?text=NB", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "nguyen.quoc.bao@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8L, "https://placehold.co/400/ffedd5/ea580c?text=HL", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "hoang.thi.lan@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9L, "https://placehold.co/400/cffafe/0891b2?text=VK", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "vu.minh.khoa@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10L, "https://placehold.co/400/fce7f3/db2777?text=DT", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "do.thi.thu@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 11L, "https://placehold.co/400/e0f2fe/0284c7?text=BL", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bui.van.long@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 12L, "https://placehold.co/400/f1f5f9/475569?text=NB", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "nguyen.thi.bich@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 13L, "https://placehold.co/400/fef2f2/dc2626?text=TN", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "trinh.van.nam@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "TEACHER", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 14L, "https://placehold.co/400/e0e7ff/4f46e5?text=S2", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "student2@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "STUDENT", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 15L, "https://placehold.co/400/ecfdf5/059669?text=S3", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "student3@learnmate.vn", true, "$2a$12$BgRmemonnrWXu0O0hABfSuRgIBIjUevBcIGTk53b.y0oPqW45tCka", "STUDENT", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "classes",
                columns: new[] { "id", "created_at", "deleted_at", "description", "max_students", "meeting_link", "name", "subject", "teacher_id", "thumbnail_url", "updated_at" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn luyện toàn bộ chương trình Toán lớp 12.", 20, null, "Toán 12 — Luyện thi THPT", "Toán", 2L, "https://placehold.co/400?text=Course", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cross-Platform Mobile App Development With .NET MAUI", 30, null, "PRN222", "PRN222", 2L, "https://placehold.co/400?text=Course", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Unity Game Development — Scripting & Physics", 30, null, "PRU213", "PRU213", 2L, "https://placehold.co/400?text=Course", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "notifications",
                columns: new[] { "id", "content", "created_at", "is_read", "title", "updated_at", "user_id" },
                values: new object[,]
                {
                    { 1L, "Bạn đã tham gia lớp Cross-Platform Mobile App Development thành công!", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Chào mừng đến PRN222", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3L },
                    { 2L, "Bạn đã tham gia lớp Unity Game Development thành công!", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Chào mừng đến PRU213", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3L }
                });

            migrationBuilder.InsertData(
                table: "notifications",
                columns: new[] { "id", "content", "created_at", "title", "updated_at", "user_id" },
                values: new object[,]
                {
                    { 3L, "Lab 1: MAUI Navigation đã được đăng. Hạn nộp: 16/03/2026.", new DateTime(2026, 3, 7, 1, 0, 0, 0, DateTimeKind.Utc), "Bài tập mới — PRN222", new DateTime(2026, 3, 7, 1, 0, 0, 0, DateTimeKind.Utc), 3L },
                    { 4L, "Lab 1: Unity Scene đã được đăng. Hạn nộp: 15/03/2026.", new DateTime(2026, 3, 7, 2, 0, 0, 0, DateTimeKind.Utc), "Bài tập mới — PRU213", new DateTime(2026, 3, 7, 2, 0, 0, 0, DateTimeKind.Utc), 3L },
                    { 5L, "PRN222: Thứ 2 & Thứ 4 lúc 07:30. PRU213: Thứ 3 & Thứ 5 lúc 13:00.", new DateTime(2026, 3, 8, 3, 0, 0, 0, DateTimeKind.Utc), "Lịch học tuần này", new DateTime(2026, 3, 8, 3, 0, 0, 0, DateTimeKind.Utc), 3L }
                });

            migrationBuilder.InsertData(
                table: "student_profiles",
                columns: new[] { "id", "created_at", "date_of_birth", "full_name", "grade_level", "parent_contact", "study_streak_days", "updated_at", "user_id" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nguyen Minh Tuan", "12", null, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3L },
                    { 2L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Le Thi Hoa", "11", null, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14L },
                    { 3L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Pham Van Kien", "10", null, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15L }
                });

            migrationBuilder.InsertData(
                table: "teacher_profiles",
                columns: new[] { "id", "admin_notes", "avatar_url", "bank_account", "bio", "created_at", "full_name", "hourly_rate", "languages_spoken", "rating_avg", "status", "subjects", "teaching_philosophy", "total_rating_count", "updated_at", "user_id", "verified_at", "years_of_experience" },
                values: new object[,]
                {
                    { 1L, null, null, null, "Giáo viên Toán với 5 năm kinh nghiệm.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nguyen Van A", 35m, null, 3.8m, "PENDING", "Toán, Vật Lý", null, 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2L, null, null },
                    { 2L, null, null, null, "Giáo viên Toán & Vật Lý với 7 năm kinh nghiệm luyện thi đại học. Hơn 200 học sinh đã đỗ các trường top.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tran Thi Mai", 28m, null, 4.5m, "PENDING", "Mathematics,Science", null, 42, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4L, null, null },
                    { 3L, null, null, null, "Thạc sĩ Hóa học, chuyên ôn luyện Hóa & Sinh cho học sinh THPT. Phương pháp dạy trực quan, dễ hiểu.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Le Van Duc", 35m, null, 4.2m, "PENDING", "Science", null, 28, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5L, null, null },
                    { 4L, null, null, null, "IELTS 8.0, 10 năm dạy Tiếng Anh giao tiếp và luyện thi IELTS/TOEIC. Cam kết đầu ra rõ ràng.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pham Thi Huong", 45m, null, 4.8m, "PENDING", "English", null, 105, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6L, null, null },
                    { 5L, null, null, null, "Kỹ sư phần mềm tại FPT Software, 5 năm dạy lập trình Python, C#, và Web Development cho mọi trình độ.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nguyen Quoc Bao", 55m, null, 4.6m, "PENDING", "Coding", null, 67, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7L, null, null },
                    { 6L, null, null, null, "Giáo viên Ngữ Văn & Lịch Sử THPT Quốc Gia. Chuyên luyện đề thi và viết văn nghị luận xã hội.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hoang Thi Lan", 22m, null, 4.0m, "PENDING", "English", null, 19, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8L, null, null },
                    { 7L, null, null, null, "Tiến sĩ Toán ứng dụng, cựu giảng viên ĐH Bách Khoa. Dạy Toán cao cấp, Thống kê và Tin học.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vu Minh Khoa", 40m, null, 4.7m, "PENDING", "Mathematics,Coding", null, 83, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9L, null, null },
                    { 8L, null, null, null, "Tốt nghiệp ĐH Ngoại Ngữ, dạy Tiếng Anh và Tiếng Pháp. 8 năm kinh nghiệm, lớp học tương tác cao.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Do Thi Thu", 38m, null, 4.3m, "PENDING", "English,Languages", null, 51, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10L, null, null },
                    { 9L, null, null, null, "Cựu học sinh Chuyên Lý ĐH Khoa Học Tự Nhiên. Dạy Vật Lý và Hóa học theo hướng tư duy phân tích.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bui Van Long", 30m, null, 4.1m, "PENDING", "Science,Mathematics", null, 33, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11L, null, null },
                    { 10L, null, null, null, "Giáo viên Toán chuyên, huy chương Bạc Olympic Toán quốc gia. Đam mê giúp học sinh yêu thích Toán học.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nguyen Thi Bich", 25m, null, 4.9m, "PENDING", "Mathematics", null, 134, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12L, null, null },
                    { 11L, null, null, null, "Senior Developer 8 năm kinh nghiệm, chuyên dạy Lập trình và Toán rời rạc cho sinh viên CNTT.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Trinh Van Nam", 65m, null, 4.4m, "PENDING", "Coding,Mathematics", null, 58, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13L, null, null }
                });

            migrationBuilder.InsertData(
                table: "assignments",
                columns: new[] { "id", "class_id", "created_at", "deleted_at", "description", "due_date", "file_url", "status", "teacher_id", "title", "updated_at" },
                values: new object[,]
                {
                    { 1L, 2L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xây dựng ứng dụng MAUI có ít nhất 3 màn hình với Shell Navigation và truyền dữ liệu giữa các trang.", new DateTime(2026, 3, 16, 16, 59, 0, 0, DateTimeKind.Utc), null, "PUBLISHED", 2L, "Lab 1: MAUI Navigation", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, 3L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tạo một scene Unity có ánh sáng directional, mặt phẳng (Plane), và ít nhất 1 GameObject có Rigidbody và Collider.", new DateTime(2026, 3, 15, 16, 59, 0, 0, DateTimeKind.Utc), null, "PUBLISHED", 2L, "Lab 1: Unity Scene", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "class_members",
                columns: new[] { "id", "class_id", "joined_at", "status", "student_id" },
                values: new object[,]
                {
                    { 1L, 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ACTIVE", 3L },
                    { 2L, 2L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ACTIVE", 3L },
                    { 3L, 3L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ACTIVE", 3L }
                });

            migrationBuilder.InsertData(
                table: "materials",
                columns: new[] { "id", "class_id", "created_at", "description", "file_size_bytes", "file_type", "file_url", "title", "updated_at", "uploaded_by" },
                values: new object[,]
                {
                    { 1L, 2L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tổng quan về .NET MAUI, XAML cơ bản và cấu trúc project.", null, "PDF", "https://storage.learnmate.vn/materials/prn222-slide-01.pdf", "Slide Buổi 1 — .NET MAUI Introduction", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2L },
                    { 2L, 3L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hướng dẫn cài đặt Unity Hub, tạo project và làm quen với Editor.", null, "PDF", "https://storage.learnmate.vn/materials/pru213-slide-01.pdf", "Slide Buổi 1 — Unity Editor Overview", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2L }
                });

            migrationBuilder.InsertData(
                table: "schedules",
                columns: new[] { "id", "class_id", "created_at", "end_time", "start_time", "title", "type", "updated_at" },
                values: new object[,]
                {
                    { 1L, 2L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 2, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 9, 0, 30, 0, 0, DateTimeKind.Utc), "PRN222 — Buổi 1: Giới thiệu .NET MAUI & XAML", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, 2L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 2, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 11, 0, 30, 0, 0, DateTimeKind.Utc), "PRN222 — Buổi 2: Data Binding & MVVM", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3L, 3L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 6, 0, 0, 0, DateTimeKind.Utc), "PRU213 — Buổi 1: Unity Editor & Scene Setup", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4L, 3L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 6, 0, 0, 0, DateTimeKind.Utc), "PRU213 — Buổi 2: Rigidbody Physics & Colliders", "REGULAR", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "assignment_questions",
                columns: new[] { "id", "assignment_id", "content", "created_at", "order", "points", "type", "updated_at" },
                values: new object[,]
                {
                    { 1L, 1L, "Triển khai Shell Navigation giữa ít nhất 3 màn hình.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 5m, "ESSAY", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, 1L, "Truyền dữ liệu giữa các trang bằng QueryProperty hoặc constructor injection.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 5m, "ESSAY", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3L, 2L, "Tạo scene Unity với đầy đủ ánh sáng, Plane, Rigidbody và Collider.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 10m, "ESSAY", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "submissions",
                columns: new[] { "id", "assignment_id", "created_at", "deleted_at", "file_url", "score", "status", "student_id", "submitted_at", "updated_at" },
                values: new object[,]
                {
                    { 1L, 1L, new DateTime(2026, 3, 14, 10, 0, 0, 0, DateTimeKind.Utc), null, null, 8.5m, "GRADED", 3L, new DateTime(2026, 3, 14, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, 2L, new DateTime(2026, 3, 14, 15, 30, 0, 0, DateTimeKind.Utc), null, null, null, "SUBMITTED", 3L, new DateTime(2026, 3, 14, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 14, 15, 30, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "feedbacks",
                columns: new[] { "id", "comment", "created_at", "score", "submission_id", "updated_at" },
                values: new object[] { 1L, "Bài làm tốt! Navigation giữa các màn hình hoạt động đúng yêu cầu. Tuy nhiên, cần cải thiện phần truyền dữ liệu — nên dùng QueryProperty thay vì singleton. Tiếp tục cố gắng nhé!", new DateTime(2026, 3, 15, 8, 0, 0, 0, DateTimeKind.Utc), 8.5m, 1L, new DateTime(2026, 3, 15, 8, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "ix_assignment_options_question_id",
                table: "assignment_options",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "ix_assignment_questions_assignment_id",
                table: "assignment_questions",
                column: "assignment_id");

            migrationBuilder.CreateIndex(
                name: "ix_assignments_class_id",
                table: "assignments",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "ix_assignments_teacher_id",
                table: "assignments",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "ix_attendances_schedule_id",
                table: "attendances",
                column: "schedule_id");

            migrationBuilder.CreateIndex(
                name: "ix_attendances_student_id",
                table: "attendances",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "ix_class_members_class_id_student_id",
                table: "class_members",
                columns: new[] { "class_id", "student_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_class_members_student_id",
                table: "class_members",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "ix_classes_teacher_id",
                table: "classes",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "ix_conversations_participant_a_id_participant_b_id",
                table: "conversations",
                columns: new[] { "participant_a_id", "participant_b_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_conversations_participant_b_id",
                table: "conversations",
                column: "participant_b_id");

            migrationBuilder.CreateIndex(
                name: "ix_feedbacks_submission_id",
                table: "feedbacks",
                column: "submission_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_invoices_class_id",
                table: "invoices",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_teacher_id",
                table: "invoices",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "ix_materials_class_id",
                table: "materials",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "ix_materials_uploaded_by",
                table: "materials",
                column: "uploaded_by");

            migrationBuilder.CreateIndex(
                name: "ix_messages_conversation_id",
                table: "messages",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "ix_messages_sender_id",
                table: "messages",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_user_id_is_read",
                table: "notifications",
                columns: new[] { "user_id", "is_read" });

            migrationBuilder.CreateIndex(
                name: "ix_otp_verifications_user_id_is_used",
                table: "otp_verifications",
                columns: new[] { "user_id", "is_used" });

            migrationBuilder.CreateIndex(
                name: "ix_payments_class_id",
                table: "payments",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "ix_payments_invoice_id",
                table: "payments",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_payments_student_id",
                table: "payments",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "ix_schedules_class_id",
                table: "schedules",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "ix_schedules_start_time",
                table: "schedules",
                column: "start_time");

            migrationBuilder.CreateIndex(
                name: "ix_student_profiles_user_id",
                table: "student_profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_submission_answer_options_option_id",
                table: "submission_answer_options",
                column: "option_id");

            migrationBuilder.CreateIndex(
                name: "ix_submission_answer_options_submission_answer_id_option_id",
                table: "submission_answer_options",
                columns: new[] { "submission_answer_id", "option_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_submission_answers_question_id",
                table: "submission_answers",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "ix_submission_answers_submission_id",
                table: "submission_answers",
                column: "submission_id");

            migrationBuilder.CreateIndex(
                name: "ix_submissions_assignment_id",
                table: "submissions",
                column: "assignment_id");

            migrationBuilder.CreateIndex(
                name: "ix_submissions_assignment_id_student_id",
                table: "submissions",
                columns: new[] { "assignment_id", "student_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_submissions_student_id",
                table: "submissions",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "ix_teacher_documents_teacher_profile_id",
                table: "teacher_documents",
                column: "teacher_profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_teacher_profiles_user_id",
                table: "teacher_profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_teacher_ratings_class_id",
                table: "teacher_ratings",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "ix_teacher_ratings_student_id_teacher_id_class_id",
                table: "teacher_ratings",
                columns: new[] { "student_id", "teacher_id", "class_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_teacher_ratings_teacher_id",
                table: "teacher_ratings",
                column: "teacher_id");

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

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_video_sessions_schedule_id",
                table: "video_sessions",
                column: "schedule_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attendances");

            migrationBuilder.DropTable(
                name: "class_members");

            migrationBuilder.DropTable(
                name: "feedbacks");

            migrationBuilder.DropTable(
                name: "materials");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "otp_verifications");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "reports");

            migrationBuilder.DropTable(
                name: "student_profiles");

            migrationBuilder.DropTable(
                name: "submission_answer_options");

            migrationBuilder.DropTable(
                name: "teacher_documents");

            migrationBuilder.DropTable(
                name: "teacher_ratings");

            migrationBuilder.DropTable(
                name: "tutor_booking_requests");

            migrationBuilder.DropTable(
                name: "video_sessions");

            migrationBuilder.DropTable(
                name: "conversations");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "assignment_options");

            migrationBuilder.DropTable(
                name: "submission_answers");

            migrationBuilder.DropTable(
                name: "teacher_profiles");

            migrationBuilder.DropTable(
                name: "schedules");

            migrationBuilder.DropTable(
                name: "assignment_questions");

            migrationBuilder.DropTable(
                name: "submissions");

            migrationBuilder.DropTable(
                name: "assignments");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
