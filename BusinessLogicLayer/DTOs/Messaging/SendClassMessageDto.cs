using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTOs.Messaging;

public class SendClassMessageDto
{
    [Required]
    [MaxLength(4000)]
    public string Content { get; set; } = string.Empty;
}
