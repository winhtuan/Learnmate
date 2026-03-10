namespace BusinessLogicLayer.DTOs.ClassManagement;

public class ClassRowDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int EnrolledCount { get; set; }
    public int MaxStudents { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
}
