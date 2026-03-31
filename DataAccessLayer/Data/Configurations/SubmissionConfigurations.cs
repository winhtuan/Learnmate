using BusinessObject.Enum;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations;

// ─── submissions ──────────────────────────────────────────────────────────────
internal sealed class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
{
    public void Configure(EntityTypeBuilder<Submission> builder)
    {
        builder.HasIndex(s => new { s.AssignmentId, s.StudentId }).IsUnique();
        builder.HasIndex(s => s.AssignmentId);
        builder.HasIndex(s => s.StudentId);

        builder
            .Property(s => s.Status)
            .HasConversion<string>()
            .HasDefaultValue(SubmissionStatus.DRAFT);

        builder.Property(s => s.Score).HasColumnType("numeric(5,2)");

        builder.ToTable(t =>
            t.HasCheckConstraint("ck_submissions_score", "score IS NULL OR score >= 0")
        );

        builder
            .HasOne(s => s.Assignment)
            .WithMany(a => a.Submissions)
            .HasForeignKey(s => s.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(s => s.Student)
            .WithMany(u => u.Submissions)
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

// ─── submission_answers ───────────────────────────────────────────────────────
internal sealed class SubmissionAnswerConfiguration : IEntityTypeConfiguration<SubmissionAnswer>
{
    public void Configure(EntityTypeBuilder<SubmissionAnswer> builder)
    {
        builder.HasIndex(sa => sa.SubmissionId);

        builder
            .HasOne(sa => sa.Submission)
            .WithMany(s => s.Answers)
            .HasForeignKey(sa => sa.SubmissionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(sa => sa.Question)
            .WithMany(q => q.SubmissionAnswers)
            .HasForeignKey(sa => sa.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

// ─── submission_answer_options ────────────────────────────────────────────────
internal sealed class SubmissionAnswerOptionConfiguration
    : IEntityTypeConfiguration<SubmissionAnswerOption>
{
    public void Configure(EntityTypeBuilder<SubmissionAnswerOption> builder)
    {
        builder.HasIndex(sao => new { sao.SubmissionAnswerId, sao.OptionId }).IsUnique();

        builder
            .HasOne(sao => sao.SubmissionAnswer)
            .WithMany(sa => sa.SelectedOptions)
            .HasForeignKey(sao => sao.SubmissionAnswerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(sao => sao.Option)
            .WithMany(o => o.SubmissionAnswerOptions)
            .HasForeignKey(sao => sao.OptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

// ─── feedbacks ────────────────────────────────────────────────────────────────
internal sealed class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasIndex(f => f.SubmissionId).IsUnique();

        builder.Property(f => f.Score).HasColumnType("numeric(5,2)");

        builder.ToTable(t => t.HasCheckConstraint("ck_feedbacks_score", "score >= 0"));

        builder
            .HasOne(f => f.Submission)
            .WithOne(s => s.Feedback)
            .HasForeignKey<Feedback>(f => f.SubmissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
