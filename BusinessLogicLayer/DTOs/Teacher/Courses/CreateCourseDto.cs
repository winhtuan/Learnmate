namespace BusinessLogicLayer.DTOs.Teacher.Courses;

public class CreateCourseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    // Add other course properties
}
