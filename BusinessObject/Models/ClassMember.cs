using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;

namespace BusinessObject.Models;

[Table("class_members")]
public class ClassMember
{
    [Key]
    public long Id { get; set; }

    public long ClassId { get; set; }

    public long StudentId { get; set; }

    [Required]
    public ClassMemberStatus Status { get; set; } = ClassMemberStatus.PENDING;

    public DateTime JoinedAt { get; set; }

    // Navigation properties
    [ForeignKey("ClassId")]
    public Class Class { get; set; } = null!;

    [ForeignKey("StudentId")]
    public User Student { get; set; } = null!;
}
