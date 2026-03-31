using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("conversations")]
public class Conversation : SoftDeletableEntity
{
    // Normalized: ParticipantAId = min(userA, userB), ParticipantBId = max(userA, userB)
    public long ParticipantAId { get; set; }
    public long ParticipantBId { get; set; }

    public DateTime? LastMessageAt { get; set; }

    [ForeignKey("ParticipantAId")]
    public User ParticipantA { get; set; } = null!;

    [ForeignKey("ParticipantBId")]
    public User ParticipantB { get; set; } = null!;

    public ICollection<Message> Messages { get; set; } = [];
}
