using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models.Base;

public abstract class AuditableEntity
{
    [Key]
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
