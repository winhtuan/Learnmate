using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations;

// ─── users ───────────────────────────────────────────────────────────────────
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Role).HasConversion<string>();
        builder.Property(u => u.IsActive).HasDefaultValue(true);
        builder.HasIndex(u => u.Email).IsUnique();
    }
}

// ─── teacher_profiles ────────────────────────────────────────────────────────
internal sealed class TeacherProfileConfiguration : IEntityTypeConfiguration<TeacherProfile>
{
    public void Configure(EntityTypeBuilder<TeacherProfile> builder)
    {
        builder.HasIndex(p => p.UserId).IsUnique();

        builder.Property(p => p.HourlyRate).HasColumnType("numeric(12,2)");
        builder.Property(p => p.RatingAvg).HasColumnType("numeric(5,2)");
        builder.Property(p => p.RatingAvg).HasDefaultValue(0m);
        builder.Property(p => p.TotalRatingCount).HasDefaultValue(0);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_teacher_profiles_hourly_rate", "hourly_rate > 0");
            t.HasCheckConstraint("ck_teacher_profiles_rating_avg", "rating_avg BETWEEN 0 AND 5");
        });

        builder
            .HasOne(p => p.User)
            .WithOne(u => u.TeacherProfile)
            .HasForeignKey<TeacherProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

// ─── student_profiles ────────────────────────────────────────────────────────
internal sealed class StudentProfileConfiguration : IEntityTypeConfiguration<StudentProfile>
{
    public void Configure(EntityTypeBuilder<StudentProfile> builder)
    {
        builder.HasIndex(p => p.UserId).IsUnique();

        builder
            .HasOne(p => p.User)
            .WithOne(u => u.StudentProfile)
            .HasForeignKey<StudentProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
