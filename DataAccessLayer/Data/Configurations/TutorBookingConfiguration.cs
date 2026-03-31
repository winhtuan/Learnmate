using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations;

public class TutorBookingConfiguration : IEntityTypeConfiguration<TutorBookingRequest>
{
    public void Configure(EntityTypeBuilder<TutorBookingRequest> b)
    {
        b.Property(x => x.Status).HasConversion<string>();

        b.HasIndex(x => new { x.StudentId, x.TeacherId });
        b.HasIndex(x => x.Status);

        b.HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Teacher)
            .WithMany()
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ResultClass)
            .WithMany()
            .HasForeignKey(x => x.ResultClassId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
