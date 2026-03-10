using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations;

// ─── teacher_ratings — self-referencing (Student + Teacher both → users) ──────
internal sealed class TeacherRatingConfiguration : IEntityTypeConfiguration<TeacherRating>
{
    public void Configure(EntityTypeBuilder<TeacherRating> builder)
    {
        builder
            .HasIndex(r => new
            {
                r.StudentId,
                r.TeacherId,
                r.ClassId,
            })
            .IsUnique();
        builder.HasIndex(r => r.TeacherId);

        builder.Property(r => r.Rating).HasColumnType("numeric(5,2)");

        builder.ToTable(t =>
            t.HasCheckConstraint("ck_teacher_ratings_rating", "rating BETWEEN 1.0 AND 5.0")
        );

        builder
            .HasOne(r => r.Student)
            .WithMany(u => u.RatingsGiven)
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(r => r.Teacher)
            .WithMany(u => u.RatingsReceived)
            .HasForeignKey(r => r.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(r => r.Class)
            .WithMany(c => c.TeacherRatings)
            .HasForeignKey(r => r.ClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
