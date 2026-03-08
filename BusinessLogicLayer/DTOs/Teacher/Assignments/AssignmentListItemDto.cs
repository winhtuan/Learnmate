using BusinessObject.Enum;

namespace BusinessLogicLayer.DTOs.Teacher.Assignments;

public class AssignmentListItemDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AssignmentStatus Status { get; set; }
    public DateTime? DueDate { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public int QuestionCount { get; set; }
    public int SubmissionCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
