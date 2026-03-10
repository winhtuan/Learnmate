using BusinessObject.Enum;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations;

// ─── materials ───────────────────────────────────────────────────────────────
internal sealed class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.HasIndex(m => m.ClassId);

        builder
            .Property(m => m.Status)
            .HasConversion<string>()
            .HasDefaultValue(MaterialStatus.ACTIVE);

        builder
            .HasOne(m => m.Class)
            .WithMany(c => c.Materials)
            .HasForeignKey(m => m.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(m => m.Uploader)
            .WithMany(u => u.Materials)
            .HasForeignKey(m => m.UploadedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
