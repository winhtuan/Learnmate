using BusinessObject.Enum;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations;

// ─── schedules ───────────────────────────────────────────────────────────────
internal sealed class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.HasIndex(s => s.ClassId);
        builder.HasIndex(s => s.StartTime);

        builder.Property(s => s.IsTrial).HasDefaultValue(false);
        builder.Property(s => s.Status)
               .HasConversion<string>()
               .HasDefaultValue(ScheduleStatus.SCHEDULED);
        builder.Property(s => s.Type).HasConversion<string>();

        builder.ToTable(t =>
            t.HasCheckConstraint("ck_schedules_time", "end_time > start_time"));

        builder.HasOne(s => s.Class)
               .WithMany(c => c.Schedules)
               .HasForeignKey(s => s.ClassId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

// ─── video_sessions ──────────────────────────────────────────────────────────
internal sealed class VideoSessionConfiguration : IEntityTypeConfiguration<VideoSession>
{
    public void Configure(EntityTypeBuilder<VideoSession> builder)
    {
        builder.HasIndex(vs => vs.ScheduleId).IsUnique();

        builder.Property(vs => vs.Provider).HasConversion<string>();
        builder.Property(vs => vs.Status)
               .HasConversion<string>()
               .HasDefaultValue(VideoSessionStatus.WAITING);

        builder.HasOne(vs => vs.Schedule)
               .WithOne(s => s.VideoSession)
               .HasForeignKey<VideoSession>(vs => vs.ScheduleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
