using BusinessObject.Enum;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations;

// ─── classes ─────────────────────────────────────────────────────────────────
internal sealed class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.Property(c => c.Status)
               .HasConversion<string>()
               .HasDefaultValue(ClassStatus.ACTIVE);

        builder.Property(c => c.MaxStudents).HasDefaultValue(30);

        builder.ToTable(t =>
            t.HasCheckConstraint("ck_classes_max_students", "max_students > 0"));

        builder.HasOne(c => c.Teacher)
               .WithMany(u => u.Classes)
               .HasForeignKey(c => c.TeacherId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

// ─── class_members ───────────────────────────────────────────────────────────
internal sealed class ClassMemberConfiguration : IEntityTypeConfiguration<ClassMember>
{
    public void Configure(EntityTypeBuilder<ClassMember> builder)
    {
        builder.HasIndex(cm => new { cm.ClassId, cm.StudentId }).IsUnique();
        builder.HasIndex(cm => cm.StudentId);

        builder.Property(cm => cm.Status)
               .HasConversion<string>()
               .HasDefaultValue(ClassMemberStatus.PENDING);

        builder.Property(cm => cm.JoinedAt).HasDefaultValueSql("NOW()");

        builder.HasOne(cm => cm.Class)
               .WithMany(c => c.ClassMembers)
               .HasForeignKey(cm => cm.ClassId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cm => cm.Student)
               .WithMany(u => u.ClassMembers)
               .HasForeignKey(cm => cm.StudentId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
