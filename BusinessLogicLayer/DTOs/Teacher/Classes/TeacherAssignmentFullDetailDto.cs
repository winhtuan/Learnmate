using BusinessObject.Enum;

namespace BusinessLogicLayer.DTOs.Teacher.Classes;

public class TeacherAssignmentFullDetailDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    
    // Class stats
    public int TotalStudents { get; set; }
    public int SubmittedCount { get; set; }
    public int GradedCount { get; set; }

    public List<TeacherSubmissionItemDto> Submissions { get; set; } = [];
}

public class TeacherSubmissionItemDto
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string? StudentAvatarUrl { get; set; }
    
    public string Status { get; set; } = string.Empty;
    public DateTime? SubmittedAt { get; set; }
    public string? FileUrl { get; set; }
    public string? FileName { get; set; }
    
    public decimal? Score { get; set; }
    public string? FeedbackComment { get; set; }
}
