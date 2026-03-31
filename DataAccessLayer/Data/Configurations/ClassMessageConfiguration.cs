using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations;

public class ClassMessageConfiguration : IEntityTypeConfiguration<ClassMessage>
{
    public void Configure(EntityTypeBuilder<ClassMessage> builder)
    {
        builder.Property(x => x.Content).HasMaxLength(4000).IsRequired();
        
        builder.HasIndex(x => x.ClassId);
        builder.HasIndex(x => x.SenderId);

        builder.HasOne(x => x.Class)
            .WithMany()
            .HasForeignKey(x => x.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Sender)
            .WithMany()
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
