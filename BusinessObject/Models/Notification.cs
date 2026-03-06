using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("notifications")]
public class Notification : AuditableEntity
{
    public long UserId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    [Required]
    public string Content { get; set; } = null!;

    public bool IsRead { get; set; } = false;

    // Navigation properties
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}
