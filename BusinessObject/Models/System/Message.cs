using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("messages")]
public class Message : AuditableEntity
{
    public long ConversationId { get; set; }
    public long SenderId { get; set; }

    [Required]
    [MaxLength(4000)]
    public string Content { get; set; } = null!;

    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }

    [ForeignKey("ConversationId")]
    public Conversation Conversation { get; set; } = null!;

    [ForeignKey("SenderId")]
    public User Sender { get; set; } = null!;
}
