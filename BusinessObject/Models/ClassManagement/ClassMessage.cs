using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("class_messages")]
public class ClassMessage : AuditableEntity
{
    public long ClassId { get; set; }
    
    public long SenderId { get; set; }

    [Required]
    [MaxLength(4000)]
    public string Content { get; set; } = null!;

    [ForeignKey("ClassId")]
    public Class Class { get; set; } = null!;

    [ForeignKey("SenderId")]
    public User Sender { get; set; } = null!;
}
