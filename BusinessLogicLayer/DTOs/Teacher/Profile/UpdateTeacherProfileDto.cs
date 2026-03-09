namespace BusinessLogicLayer.DTOs.Teacher.Profile;

public class UpdateTeacherProfileDto
{
    public string FullName { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    // Add other profile properties here (e.g., AvatarUrl, PhoneNumber)
}
