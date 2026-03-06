namespace BusinessObject.Models.Base;

public abstract class SoftDeletableEntity : AuditableEntity
{
    public DateTime? DeletedAt { get; set; }
}
