using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> b)
    {
        // One unique conversation per pair — normalized (A = min, B = max)
        b.HasIndex(x => new { x.ParticipantAId, x.ParticipantBId }).IsUnique();

        b.HasOne(x => x.ParticipantA)
            .WithMany()
            .HasForeignKey(x => x.ParticipantAId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ParticipantB)
            .WithMany()
            .HasForeignKey(x => x.ParticipantBId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> b)
    {
        b.Property(x => x.Content).HasMaxLength(4000);
        b.HasIndex(x => x.ConversationId);
        b.HasIndex(x => x.SenderId);

        b.HasOne(x => x.Sender)
            .WithMany()
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
