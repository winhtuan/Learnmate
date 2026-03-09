using BusinessObject.Enum;

namespace BusinessLogicLayer.DTOs.Teacher.Assignments;

public class AssignmentDetailDto
{
    public long Id { get; set; }
    public long ClassId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AssignmentStatus Status { get; set; }
    public DateTime? DueDate { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<AssignmentQuestionDetailDto> Questions { get; set; } = new();
}

public class AssignmentQuestionDetailDto
{
    public long Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public QuestionType Type { get; set; }
    public int Order { get; set; }
    public decimal Points { get; set; }
    public List<AssignmentOptionDetailDto> Options { get; set; } = new();
}

public class AssignmentOptionDetailDto
{
    public long Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int Order { get; set; }
}
