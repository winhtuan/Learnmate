using BusinessObject.Enum;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations;

// ─── assignments ─────────────────────────────────────────────────────────────
internal sealed class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.HasIndex(a => a.ClassId);

        builder
            .Property(a => a.Status)
            .HasConversion<string>()
            .HasDefaultValue(AssignmentStatus.DRAFT);

        builder
            .HasOne(a => a.Class)
            .WithMany(c => c.Assignments)
            .HasForeignKey(a => a.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(a => a.Teacher)
            .WithMany(u => u.Assignments)
            .HasForeignKey(a => a.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

// ─── assignment_questions ─────────────────────────────────────────────────────
internal sealed class AssignmentQuestionConfiguration : IEntityTypeConfiguration<AssignmentQuestion>
{
    public void Configure(EntityTypeBuilder<AssignmentQuestion> builder)
    {
        builder.Property(q => q.Type).HasConversion<string>();
        builder.Property(q => q.Points).HasColumnType("numeric(5,2)");

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_assignment_questions_points", "points > 0");
            t.HasCheckConstraint("ck_assignment_questions_order", "\"order\" > 0");
        });

        builder
            .HasOne(q => q.Assignment)
            .WithMany(a => a.Questions)
            .HasForeignKey(q => q.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

// ─── assignment_options ───────────────────────────────────────────────────────
internal sealed class AssignmentOptionConfiguration : IEntityTypeConfiguration<AssignmentOption>
{
    public void Configure(EntityTypeBuilder<AssignmentOption> builder)
    {
        builder.Property(o => o.IsCorrect).HasDefaultValue(false);

        builder.ToTable(t => t.HasCheckConstraint("ck_assignment_options_order", "\"order\" > 0"));

        builder
            .HasOne(o => o.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
